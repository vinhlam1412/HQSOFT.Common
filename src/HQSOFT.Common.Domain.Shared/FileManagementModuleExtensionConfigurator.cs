using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;
using Volo.FileManagement.Files;
using Volo.ObjectExtending;

namespace HQSOFT.Common
{
    public class FileManagementModuleExtensionConfigurator
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                ConfigureExtraProperties();
            });
        }

        public static void ConfigureExtraProperties()
        {
            ObjectExtensionManager.Instance.Modules()
                .ConfigureFileManagement(identity =>
                {
                    identity.ConfigureFileDescriptor(file =>
                    {
                        file.AddOrUpdateProperty<string?>(
                            "Url", //property name
                            property =>
                            {
                                property.Attributes.Add(new RequiredAttribute());
                            }

                        );
                    });

                    identity.ConfigureFileDescriptor(user =>
                    {
                        user.AddOrUpdateProperty<Guid>(
                            "DocId", //property name
                            property =>
                            {
                                property.Attributes.Add(new RequiredAttribute());
                                property.DefaultValue = Guid.Empty; 
                            }

                        );
                    });
                });

			ObjectExtensionManager.Instance.Modules()
				 .ConfigureAuditLogging(auditlogging =>
				 {
					 auditlogging.ConfigureAuditLog(audit =>
					 {
						 audit.AddOrUpdateProperty<string>(  
							 "ScreenUrl"
						 );
					 }); 

                     auditlogging.ConfigureEntityChange(auditaction =>
                     {
                         auditaction.AddOrUpdateProperty<string>(
                             "ScreenUrl"
                         );
                     });

					 auditlogging.ConfigureEntityChange(auditaction =>
					 {
						 auditaction.AddOrUpdateProperty<Guid?>(  
							 "UserId", 
							property =>
							{ 
								property.DefaultValue = Guid.Empty;
							} 
						 );
					 });
				 }); 
		}
	}
}
