using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace SOAPWebServiceASP.NET472ToPCF
{
    public class PortReflector : SoapExtensionReflector
    {
        public override void ReflectMethod()
        {
            // NO-OPERATION;
        }

        public override void ReflectDescription()
        {
            ServiceDescription description = ReflectionContext.ServiceDescription;

            foreach (Service service in description.Services)
            {
                foreach(Port port in service.Ports)
                {
                    foreach(ServiceDescriptionFormatExtension extension in port.Extensions)
                    {
                        SoapAddressBinding binding = extension as SoapAddressBinding;
                        if (null != binding)
                        {
                            Uri locationWithPort = new Uri(binding.Location);
                            UriBuilder builder = new UriBuilder(locationWithPort);
                            builder.Port = -1;
                            Uri locationWithoutPort = builder.Uri;
                            binding.Location = locationWithoutPort.AbsoluteUri;
                        }
                    }
                }
            }
        }
    }
}