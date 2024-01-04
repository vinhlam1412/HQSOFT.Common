using System;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.Http.Modeling;

namespace HQSOFT.Common.Blazor.Extraproperties;

public class ExtraPropertyDictionaryToQueryString : IObjectToQueryString<ExtraPropertyDictionary>, ITransientDependency
{
    public Task<string> ConvertAsync(ActionApiDescriptionModel actionApiDescription, ParameterApiDescriptionModel parameterApiDescription, ExtraPropertyDictionary dictionary)
    {
        var sb = new StringBuilder();
        foreach (var keyValue in dictionary)
        {
            sb.Append($"ExtraProperties[{keyValue.Key}]={keyValue.Value.ToString()}&");
        }
		if (sb.Length > 0)
		{
			sb.Remove(sb.Length - 1, 1);
		}
		return Task.FromResult(sb.ToString());
    }
}
