using System;
using System.Web.Services.Description;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Net;
using System.Configuration;

namespace MetroLesMonitor
{
      [Serializable]  
   public class ServiceCallRoutines
    {
        
        string sPassword = ""; string sUser = ""; string sHost = ""; string sPort = ""; string sDomain = "";
        bool RequireAuthentication = false;
        CompilerParameters parms = null;
        string[] assemblyReferences = null;

        public ServiceCallRoutines()
        {
            assemblyReferences = new string[5] { "System.dll", "System.Web.Services.dll", "System.Web.dll", "System.Xml.dll", "System.Data.dll" };
            parms = new CompilerParameters(assemblyReferences);

            if (ConfigurationManager.AppSettings["REQUIREAUTHENTICATION"].ToUpper() == "TRUE")
            {
                RequireAuthentication = true;
                sHost = ConfigurationManager.AppSettings["PROXY_HOST"];
                sUser = ConfigurationManager.AppSettings["PROXY_USERNAME"];
                sPassword = ConfigurationManager.AppSettings["PROXY_PASSWORD"];
                sDomain = ConfigurationManager.AppSettings["PROXY_DOMAIN"];
                sPort = ConfigurationManager.AppSettings["PROXY_PORT"];
            }
        }

        public Object CallWebService(string webServiceAsmxUrl, string serviceName, string methodName, object[] args)
        {
                 System.IO.Stream stream =null;
        
                System.Net.WebClient client = new System.Net.WebClient();
                IWebProxy myProxy = null;
                if (RequireAuthentication)
                {
                    myProxy = new WebProxy(sHost + ":" + sPort, true);
                    myProxy.Credentials = new NetworkCredential(sUser, sPassword, sDomain);
                    client.Proxy = myProxy;
                }
                ServicePointManager.Expect100Continue = false;
                ServicePointManager.MaxServicePointIdleTime = 2000;
                try
                {
                     stream = client.OpenRead(webServiceAsmxUrl + "?wsdl");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        string strEx = ex.InnerException.Message;
                    }
                    throw;
                }
      
            // Now read the WSDL file describing a service.
            ServiceDescription description = ServiceDescription.Read(stream);

            ///// LOAD THE DOM /////////
            // Initialize a service description importer.
            
            ServiceDescriptionImporter importer = new ServiceDescriptionImporter();
            importer.ProtocolName = "Soap12"; // Use SOAP 1.2.

            importer.AddServiceDescription(description, null, null);


            // Generate a proxy client.
            importer.Style = ServiceDescriptionImportStyle.Client;

            // Generate properties to represent primitive values.
            importer.CodeGenerationOptions = System.Xml.Serialization.CodeGenerationOptions.GenerateProperties;

            // Initialize a Code-DOM tree into which we will import the service.
            CodeNamespace nmspace = new CodeNamespace();
            CodeCompileUnit unit1 = new CodeCompileUnit();
            unit1.Namespaces.Add(nmspace);

            // Import the service into the Code-DOM tree. This creates proxy code that uses the service.
            ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit1);

            if (warning == 0) // If zero then we are good to go
            {
                // Generate the proxy code
                CodeDomProvider provider1 = CodeDomProvider.CreateProvider("CSharp");

                // Compile the assembly proxy with the appropriate references
                //string[] assemblyReferences = new string[5] { "System.dll", "System.Web.Services.dll", "System.Web.dll", "System.Xml.dll", "System.Data.dll" };
                //CompilerParameters parms = new CompilerParameters(assemblyReferences);
                CompilerResults results = provider1.CompileAssemblyFromDom(parms, unit1);

                // Check For Errors
                //if (results.Errors.Count > 0)
                //{
                //    foreach (CompilerError oops in results.Errors)
                //    {
                //        System.Diagnostics.Debug.WriteLine("========Compiler error============");
                //        System.Diagnostics.Debug.WriteLine(oops.ErrorText);
                //    }
                //    throw new System.Exception("Compile Error Occured calling webservice. Check Debug ouput window.");
                //}

                {
                    {

                        object wsvcClass = results.CompiledAssembly.CreateInstance(serviceName);
                        if (myProxy != null) ((System.Web.Services.Protocols.HttpWebClientProtocol)(wsvcClass)).Proxy = myProxy;
                        MethodInfo mi = wsvcClass.GetType().GetMethod(methodName);
                        try
                        {
                            return mi.Invoke(wsvcClass, BindingFlags.InvokeMethod, null, args, null);
                            wsvcClass = null; mi = null;//added on 17-12-15
                            //return mi.Invoke(proxy, args);
                        }
                        catch (Exception ex)
                        {
                           // GlobalSettings.checkConnection = false;
                           
                            if (ex.InnerException != null)
                            {
                                string strEx = ex.InnerException.Message;
                            }
                            throw;
                        }
                        finally
                        {
                            GC.Collect();
                        }
                    }

                }
            }
            else
            {
                return null;
            }
            client.Dispose();
            stream.Flush(); stream.Dispose();//added on 17-12-15
            importer = null; myProxy = null; args = null;//added on 17-12-15
        }
    }
}
