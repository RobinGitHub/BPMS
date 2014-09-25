using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Design;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using WsdlNS = System.Web.Services.Description;
using System.Web.Services.Discovery;

namespace BPMS.Common
{

    internal class Constants
    {
        internal class ErrorMessages
        {
            internal const string ImportError =
                "There was an error in importing the metadata.";

            internal const string CodeGenerationError =
                "There was an error in generating the proxy code.";

            internal const string CompilationError =
                "There was an error in compiling the proxy code.";

            internal const string UnknownContract =
                "The specified contract is not found in the proxy assembly.";

            internal const string EndpointNotFound =
                "The endpoint associated with contract {1}:{0} is not found.";

            internal const string ProxyTypeNotFound =
                "The proxy that implements the service contract {0} is not found.";

            internal const string ProxyCtorNotFound =
                "The constructor matching the specified parameter types is not found.";

            internal const string ParameterValueMistmatch =
                "The type for each parameter values must be specified.";

            internal const string MethodNotFound =
                "The method {0} is not found.";
        }
    }

    public class DynamicObject
    {
        private Type objType;
        private object obj;

        private BindingFlags CommonBindingFlags =
            BindingFlags.Instance |
            BindingFlags.Public;

        public DynamicObject(Object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            this.obj = obj;
            this.objType = obj.GetType();
        }

        public DynamicObject(Type objType)
        {
            if (objType == null)
                throw new ArgumentNullException("objType");

            this.objType = objType;
        }

        public void CallConstructor()
        {
            CallConstructor(new Type[0], new object[0]);
        }

        public void CallConstructor(Type[] paramTypes, object[] paramValues)
        {
            ConstructorInfo ctor = this.objType.GetConstructor(paramTypes);
            if (ctor == null)
            {
                throw new DynamicProxyException(
                        Constants.ErrorMessages.ProxyCtorNotFound);
            }

            this.obj = ctor.Invoke(paramValues);
        }

        public object GetInstance()
        {
            if (this.objType != null)
            {
                if (this.obj == null)
                {
                    this.CallConstructor();
                }
                return this.obj;
            }
            return null;
        }

        public object GetProperty(string property)
        {
            object retval = this.objType.InvokeMember(
                property,
                BindingFlags.GetProperty | CommonBindingFlags,
                null /* Binder */,
                this.obj,
                null /* args */);

            return retval;
        }

        public object SetProperty(string property, object value)
        {
            object retval = this.objType.InvokeMember(
                property,
                BindingFlags.SetProperty | CommonBindingFlags,
                null /* Binder */,
                this.obj,
                new object[] { value });

            return retval;
        }

        public object GetField(string field)
        {
            object retval = this.objType.InvokeMember(
                field,
                BindingFlags.GetField | CommonBindingFlags,
                null /* Binder */,
                this.obj,
                null /* args */);

            return retval;
        }

        public object SetField(string field, object value)
        {
            object retval = this.objType.InvokeMember(
                field,
                BindingFlags.SetField | CommonBindingFlags,
                null /* Binder */,
                this.obj,
                new object[] { value });

            return retval;
        }

        public object CallMethod(string method, params object[] parameters)
        {
            object retval = this.objType.InvokeMember(
                method,
                BindingFlags.InvokeMethod | CommonBindingFlags,
                null /* Binder */,
                this.obj,
                parameters /* args */);

            return retval;
        }

        public object CallMethod(string method, Type[] types,
            object[] parameters)
        {
            if (types.Length != parameters.Length)
                throw new ArgumentException(
                    Constants.ErrorMessages.ParameterValueMistmatch);

            MethodInfo mi = this.objType.GetMethod(method, types);
            if (mi == null)
                throw new ApplicationException(string.Format(
                    Constants.ErrorMessages.MethodNotFound, method));

            object retval = mi.Invoke(this.obj, CommonBindingFlags, null,
                parameters, null);

            return retval;
        }

        public Type ObjectType
        {
            get
            {
                return this.objType;
            }
        }

        public object ObjectInstance
        {
            get
            {
                return this.obj;
            }
        }

        public BindingFlags BindingFlags
        {
            get
            {
                return this.CommonBindingFlags;
            }

            set
            {
                this.CommonBindingFlags = value;
            }
        }
    }

    public class DynamicProxy : DynamicObject
    {
        public DynamicProxy(Type proxyType, Binding binding,
                EndpointAddress address)
            : base(proxyType)
        {
            Type[] paramTypes = new Type[2];
            paramTypes[0] = typeof(Binding);
            paramTypes[1] = typeof(EndpointAddress);

            if (binding is BasicHttpBinding)
            {
                var bhb = binding as BasicHttpBinding;
                bhb.ReaderQuotas.MaxStringContentLength = 819200;
            }

            object[] paramValues = new object[2];
            paramValues[0] = binding;
            paramValues[1] = address;

            CallConstructor(paramTypes, paramValues);
        }

        public Type ProxyType
        {
            get
            {
                return ObjectType;
            }
        }

        public object Proxy
        {
            get
            {
                return ObjectInstance;
            }
        }

        public void Close()
        {
            CallMethod("Close");
        }
    }

    public class DynamicProxyException : ApplicationException
    {
        private IEnumerable<MetadataConversionError> importErrors = null;
        private IEnumerable<MetadataConversionError> codegenErrors = null;
        private IEnumerable<CompilerError> compilerErrors = null;

        public DynamicProxyException(string message)
            : base(message)
        {
        }

        public DynamicProxyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public IEnumerable<MetadataConversionError> MetadataImportErrors
        {
            get
            {
                return this.importErrors;
            }

            internal set
            {
                this.importErrors = value;
            }
        }

        public IEnumerable<MetadataConversionError> CodeGenerationErrors
        {
            get
            {
                return this.codegenErrors;
            }

            internal set
            {
                this.codegenErrors = value;
            }
        }

        public IEnumerable<CompilerError> CompilationErrors
        {
            get
            {
                return this.compilerErrors;
            }

            internal set
            {
                this.compilerErrors = value;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(base.ToString());

            if (MetadataImportErrors != null)
            {
                builder.AppendLine("Metadata Import Errors:");
                builder.AppendLine(DynamicProxyFactory.ToString(
                            MetadataImportErrors));
            }

            if (CodeGenerationErrors != null)
            {
                builder.AppendLine("Code Generation Errors:");
                builder.AppendLine(DynamicProxyFactory.ToString(
                            CodeGenerationErrors));
            }

            if (CompilationErrors != null)
            {
                builder.AppendLine("Compilation Errors:");
                builder.AppendLine(DynamicProxyFactory.ToString(
                            CompilationErrors));
            }

            return builder.ToString();
        }
    }

    public class DynamicProxyFactory
    {
        private string wsdlUri;
        private DynamicProxyFactoryOptions options;

        private CodeCompileUnit codeCompileUnit;
        private CodeDomProvider codeDomProvider;
        private ServiceContractGenerator contractGenerator;

        private Collection<MetadataSection> metadataCollection;
        private IEnumerable<Binding> bindings;
        private IEnumerable<ContractDescription> contracts;
        private ServiceEndpointCollection endpoints;
        private IEnumerable<MetadataConversionError> importWarnings;
        private IEnumerable<MetadataConversionError> codegenWarnings;
        private IEnumerable<CompilerError> compilerWarnings;

        private Assembly proxyAssembly;
        private string proxyCode;

        public DynamicProxyFactory(string wsdlUri, DynamicProxyFactoryOptions options)
        {
            if (wsdlUri == null)
                throw new ArgumentNullException("wsdlUri");

            if (options == null)
                throw new ArgumentNullException("options");

            this.wsdlUri = wsdlUri;
            this.options = options;

            DownloadMetadata();
            ImportMetadata();
            CreateProxy();
            WriteCode();
            CompileProxy();
        }

        public DynamicProxyFactory(string wsdlUri)
            : this(wsdlUri, new DynamicProxyFactoryOptions())
        {
        }

        private void DownloadMetadata()
        {
            EndpointAddress epr = new EndpointAddress(this.wsdlUri);

            DiscoveryClientProtocol disco = new DiscoveryClientProtocol();
            disco.AllowAutoRedirect = true;
            disco.UseDefaultCredentials = true;
            disco.DiscoverAny(this.wsdlUri);
            disco.ResolveAll();

            Collection<MetadataSection> results = new Collection<MetadataSection>();
            foreach (object document in disco.Documents.Values)
            {
                AddDocumentToResults(document, results);
            }
            this.metadataCollection = results;
        }

        void AddDocumentToResults(object document, Collection<MetadataSection> results)
        {
            WsdlNS.ServiceDescription wsdl = document as WsdlNS.ServiceDescription;
            XmlSchema schema = document as XmlSchema;
            XmlElement xmlDoc = document as XmlElement;

            if (wsdl != null)
            {
                results.Add(MetadataSection.CreateFromServiceDescription(wsdl));
            }
            else if (schema != null)
            {
                results.Add(MetadataSection.CreateFromSchema(schema));
            }
            else if (xmlDoc != null && xmlDoc.LocalName == "Policy")
            {
                results.Add(MetadataSection.CreateFromPolicy(xmlDoc, null));
            }
            else
            {
                MetadataSection mexDoc = new MetadataSection();
                mexDoc.Metadata = document;
                results.Add(mexDoc);
            }
        }


        private void ImportMetadata()
        {
            this.codeCompileUnit = new CodeCompileUnit();
            CreateCodeDomProvider();

            WsdlImporter importer = new WsdlImporter(new MetadataSet(metadataCollection));
            AddStateForDataContractSerializerImport(importer);
            AddStateForXmlSerializerImport(importer);

            this.bindings = importer.ImportAllBindings();
            this.contracts = importer.ImportAllContracts();
            this.endpoints = importer.ImportAllEndpoints();
            this.importWarnings = importer.Errors;

            bool success = true;
            if (this.importWarnings != null)
            {
                foreach (MetadataConversionError error in this.importWarnings)
                {
                    if (!error.IsWarning)
                    {
                        success = false;
                        break;
                    }
                }
            }

            if (!success)
            {
                DynamicProxyException exception = new DynamicProxyException(
                    Constants.ErrorMessages.ImportError);
                exception.MetadataImportErrors = this.importWarnings;
                throw exception;
            }
        }

        void AddStateForXmlSerializerImport(WsdlImporter importer)
        {
            XmlSerializerImportOptions importOptions =
                new XmlSerializerImportOptions(this.codeCompileUnit);
            importOptions.CodeProvider = this.codeDomProvider;

            importOptions.WebReferenceOptions = new WsdlNS.WebReferenceOptions();
            importOptions.WebReferenceOptions.CodeGenerationOptions =
                CodeGenerationOptions.GenerateProperties |
                CodeGenerationOptions.GenerateOrder;

            importOptions.WebReferenceOptions.SchemaImporterExtensions.Add(
                typeof(TypedDataSetSchemaImporterExtension).AssemblyQualifiedName);
            importOptions.WebReferenceOptions.SchemaImporterExtensions.Add(
                typeof(DataSetSchemaImporterExtension).AssemblyQualifiedName);

            importer.State.Add(typeof(XmlSerializerImportOptions), importOptions);
        }

        void AddStateForDataContractSerializerImport(WsdlImporter importer)
        {
            XsdDataContractImporter xsdDataContractImporter =
                new XsdDataContractImporter(this.codeCompileUnit);
            xsdDataContractImporter.Options = new ImportOptions();
            xsdDataContractImporter.Options.ImportXmlType =
                (this.options.FormatMode ==
                    DynamicProxyFactoryOptions.FormatModeOptions.DataContractSerializer);

            xsdDataContractImporter.Options.CodeProvider = this.codeDomProvider;
            importer.State.Add(typeof(XsdDataContractImporter),
                    xsdDataContractImporter);

            foreach (IWsdlImportExtension importExtension in importer.WsdlImportExtensions)
            {
                DataContractSerializerMessageContractImporter dcConverter =
                    importExtension as DataContractSerializerMessageContractImporter;

                if (dcConverter != null)
                {
                    if (this.options.FormatMode ==
                        DynamicProxyFactoryOptions.FormatModeOptions.XmlSerializer)
                        dcConverter.Enabled = false;
                    else
                        dcConverter.Enabled = true;
                }

            }
        }

        private void CreateProxy()
        {
            CreateServiceContractGenerator();

            foreach (ContractDescription contract in this.contracts)
            {
                this.contractGenerator.GenerateServiceContractType(contract);
            }

            bool success = true;
            this.codegenWarnings = this.contractGenerator.Errors;
            if (this.codegenWarnings != null)
            {
                foreach (MetadataConversionError error in this.codegenWarnings)
                {
                    if (!error.IsWarning)
                    {
                        success = false;
                        break;
                    }
                }
            }

            if (!success)
            {
                DynamicProxyException exception = new DynamicProxyException(
                 Constants.ErrorMessages.CodeGenerationError);
                exception.CodeGenerationErrors = this.codegenWarnings;
                throw exception;
            }
        }

        private void CompileProxy()
        {
            // reference the required assemblies with the correct path.
            CompilerParameters compilerParams = new CompilerParameters();

            AddAssemblyReference(
                typeof(System.ServiceModel.ServiceContractAttribute).Assembly,
                compilerParams.ReferencedAssemblies);

            AddAssemblyReference(
                typeof(System.Web.Services.Description.ServiceDescription).Assembly,
                compilerParams.ReferencedAssemblies);

            AddAssemblyReference(
                typeof(System.Runtime.Serialization.DataContractAttribute).Assembly,
                compilerParams.ReferencedAssemblies);

            AddAssemblyReference(typeof(System.Xml.XmlElement).Assembly,
                compilerParams.ReferencedAssemblies);

            AddAssemblyReference(typeof(System.Uri).Assembly,
                compilerParams.ReferencedAssemblies);

            AddAssemblyReference(typeof(System.Data.DataSet).Assembly,
                compilerParams.ReferencedAssemblies);

            CompilerResults results =
                this.codeDomProvider.CompileAssemblyFromSource(
                    compilerParams,
                    this.proxyCode);

            if ((results.Errors != null) && (results.Errors.HasErrors))
            {
                DynamicProxyException exception = new DynamicProxyException(
                    Constants.ErrorMessages.CompilationError);
                exception.CompilationErrors = ToEnumerable(results.Errors);

                throw exception;
            }

            this.compilerWarnings = ToEnumerable(results.Errors);
            this.proxyAssembly = Assembly.LoadFile(results.PathToAssembly);
        }

        private void WriteCode()
        {
            using (StringWriter writer = new StringWriter())
            {
                CodeGeneratorOptions codeGenOptions = new CodeGeneratorOptions();
                codeGenOptions.BracingStyle = "C";
                this.codeDomProvider.GenerateCodeFromCompileUnit(
                        this.codeCompileUnit, writer, codeGenOptions);
                writer.Flush();
                this.proxyCode = writer.ToString();
            }

            // use the modified proxy code, if code modifier is set.
            if (this.options.CodeModifier != null)
                this.proxyCode = this.options.CodeModifier(this.proxyCode);
        }

        void AddAssemblyReference(Assembly referencedAssembly,
            StringCollection refAssemblies)
        {
            string path = Path.GetFullPath(referencedAssembly.Location);
            string name = Path.GetFileName(path);
            if (!(refAssemblies.Contains(name) ||
                  refAssemblies.Contains(path)))
            {
                refAssemblies.Add(path);
            }
        }

        public ServiceEndpoint GetEndpoint(string contractName)
        {
            return GetEndpoint(contractName, null);
        }

        public ServiceEndpoint GetEndpoint(string contractName,
                string contractNamespace)
        {
            ServiceEndpoint matchingEndpoint = null;

            foreach (ServiceEndpoint endpoint in Endpoints)
            {
                if (ContractNameMatch(endpoint.Contract, contractName) &&
                    ContractNsMatch(endpoint.Contract, contractNamespace))
                {
                    matchingEndpoint = endpoint;
                    break;
                }
            }

            if (matchingEndpoint == null)
                throw new ArgumentException(string.Format(
                    Constants.ErrorMessages.EndpointNotFound,
                    contractName, contractNamespace));

            return matchingEndpoint;
        }

        private bool ContractNameMatch(ContractDescription cDesc, string name)
        {
            return (string.Compare(cDesc.Name, name, true) == 0);
        }

        private bool ContractNsMatch(ContractDescription cDesc, string ns)
        {
            return ((ns == null) ||
                    (string.Compare(cDesc.Namespace, ns, true) == 0));
        }

        public DynamicProxy CreateProxy(string contractName)
        {
            return CreateProxy(contractName, null);
        }

        public DynamicProxy CreateProxy(string contractName,
                string contractNamespace)
        {
            ServiceEndpoint endpoint = GetEndpoint(contractName,
                    contractNamespace);

            return CreateProxy(endpoint);
        }

        public DynamicProxy CreateProxy(ServiceEndpoint endpoint)
        {
            Type contractType = GetContractType(endpoint.Contract.Name,
                endpoint.Contract.Namespace);

            Type proxyType = GetProxyType(contractType);

            DynamicProxy obj = new DynamicProxy(proxyType, endpoint.Binding, endpoint.Address);
            ((ServiceEndpoint)obj.Proxy.GetType().GetProperty("Endpoint").GetValue(obj.Proxy, null)).Address = new EndpointAddress(this.wsdlUri);
            return obj;
        }

        private Type GetContractType(string contractName,
                string contractNamespace)
        {
            Type[] allTypes = proxyAssembly.GetTypes();
            ServiceContractAttribute scAttr = null;
            Type contractType = null;
            XmlQualifiedName cName;
            foreach (Type type in allTypes)
            {
                // Is it an interface?
                if (!type.IsInterface) continue;

                // Is it marked with ServiceContract attribute?
                object[] attrs = type.GetCustomAttributes(
                    typeof(ServiceContractAttribute), false);
                if ((attrs == null) || (attrs.Length == 0)) continue;

                // is it the required service contract?
                scAttr = (ServiceContractAttribute)attrs[0];
                cName = GetContractName(type, scAttr.Name, scAttr.Namespace);

                if (string.Compare(cName.Name, contractName, true) != 0)
                    continue;

                if (string.Compare(cName.Namespace, contractNamespace,
                            true) != 0)
                    continue;

                contractType = type;
                break;
            }

            if (contractType == null)
                throw new ArgumentException(
                    Constants.ErrorMessages.UnknownContract);

            return contractType;
        }

        internal const string DefaultNamespace = "http://tempuri.org/";
        internal static XmlQualifiedName GetContractName(Type contractType,
            string name, string ns)
        {
            if (String.IsNullOrEmpty(name))
            {
                name = contractType.Name;
            }

            if (ns == null)
            {
                ns = DefaultNamespace;
            }
            else
            {
                ns = Uri.EscapeUriString(ns);
            }

            return new XmlQualifiedName(name, ns);
        }

        private Type GetProxyType(Type contractType)
        {
            Type clientBaseType = typeof(ClientBase<>).MakeGenericType(
                    contractType);

            Type[] allTypes = ProxyAssembly.GetTypes();
            Type proxyType = null;

            foreach (Type type in allTypes)
            {
                // Look for a proxy class that implements the service 
                // contract and is derived from ClientBase<service contract>
                if (type.IsClass && contractType.IsAssignableFrom(type)
                    && type.IsSubclassOf(clientBaseType))
                {
                    proxyType = type;
                    break;
                }
            }

            if (proxyType == null)
                throw new DynamicProxyException(string.Format(
                            Constants.ErrorMessages.ProxyTypeNotFound,
                            contractType.FullName));

            return proxyType;
        }


        private void CreateCodeDomProvider()
        {
            this.codeDomProvider = CodeDomProvider.CreateProvider(options.Language.ToString());
        }

        private void CreateServiceContractGenerator()
        {
            this.contractGenerator = new ServiceContractGenerator(
                this.codeCompileUnit);
            this.contractGenerator.Options |= ServiceContractGenerationOptions.ClientClass;
        }

        public IEnumerable<MetadataSection> Metadata
        {
            get
            {
                return this.metadataCollection;
            }
        }

        public IEnumerable<Binding> Bindings
        {
            get
            {
                return this.bindings;
            }
        }

        public IEnumerable<ContractDescription> Contracts
        {
            get
            {
                return this.contracts;
            }
        }

        public IEnumerable<ServiceEndpoint> Endpoints
        {
            get
            {
                return this.endpoints;
            }
        }

        public Assembly ProxyAssembly
        {
            get
            {
                return this.proxyAssembly;
            }
        }

        public string ProxyCode
        {
            get
            {
                return this.proxyCode;
            }
        }

        public IEnumerable<MetadataConversionError> MetadataImportWarnings
        {
            get
            {
                return this.importWarnings;
            }
        }

        public IEnumerable<MetadataConversionError> CodeGenerationWarnings
        {
            get
            {
                return this.codegenWarnings;
            }
        }

        public IEnumerable<CompilerError> CompilationWarnings
        {
            get
            {
                return this.compilerWarnings;
            }
        }

        public static string ToString(IEnumerable<MetadataConversionError>
            importErrors)
        {
            if (importErrors != null)
            {
                StringBuilder importErrStr = new StringBuilder();

                foreach (MetadataConversionError error in importErrors)
                {
                    if (error.IsWarning)
                        importErrStr.AppendLine("Warning : " + error.Message);
                    else
                        importErrStr.AppendLine("Error : " + error.Message);
                }

                return importErrStr.ToString();
            }
            else
            {
                return null;
            }
        }

        public static string ToString(IEnumerable<CompilerError> compilerErrors)
        {
            if (compilerErrors != null)
            {
                StringBuilder builder = new StringBuilder();
                foreach (CompilerError error in compilerErrors)
                    builder.AppendLine(error.ToString());

                return builder.ToString();
            }
            else
            {
                return null;
            }
        }

        private static IEnumerable<CompilerError> ToEnumerable(
                CompilerErrorCollection collection)
        {
            if (collection == null) return null;

            List<CompilerError> errorList = new List<CompilerError>();
            foreach (CompilerError error in collection)
                errorList.Add(error);

            return errorList;
        }
    }

    public delegate string ProxyCodeModifier(string proxyCode);

    public class DynamicProxyFactoryOptions
    {
        public enum LanguageOptions { CS, VB };
        public enum FormatModeOptions { Auto, XmlSerializer, DataContractSerializer }

        private LanguageOptions lang;
        private FormatModeOptions mode;
        private ProxyCodeModifier codeModifier;

        public DynamicProxyFactoryOptions()
        {
            this.lang = LanguageOptions.CS;
            this.mode = FormatModeOptions.Auto;
            this.codeModifier = null;
        }

        public LanguageOptions Language
        {
            get
            {
                return this.lang;
            }

            set
            {
                this.lang = value;
            }
        }

        public FormatModeOptions FormatMode
        {
            get
            {
                return this.mode;
            }

            set
            {
                this.mode = value;
            }
        }

        // The code modifier allows the user of the dynamic proxy factory to modify 
        // the generated proxy code before it is compiled and used. This is useful in 
        // situations where the generated proxy has to be modified manually for interop 
        // reason.
        public ProxyCodeModifier CodeModifier
        {
            get
            {
                return this.codeModifier;
            }

            set
            {
                this.codeModifier = value;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DynamicProxyFactoryOptions[");
            sb.Append("Language=" + Language);
            sb.Append(",FormatMode=" + FormatMode);
            sb.Append(",CodeModifier=" + CodeModifier);
            sb.Append("]");

            return sb.ToString();
        }
    }
}
