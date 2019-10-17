using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebAPI_v1.Helpers
{
    public class FormFileUploadOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            ApiParameterDescription[] array = context.ApiDescription.ParameterDescriptions.Where(x => x.ParameterDescriptor.ParameterType == typeof(IFormFile)).ToArray();
            bool Predicate(ApiParameterDescription x) => x.Source.Id != "FormFile";
            if (array.Any(Predicate))
                throw new InvalidOperationException("IFormFile argument of controller's method should not be marked by binding attribute such as [FromForm], [FromBody] or etc. to be compatible with swagger");
            if (array.Any())
                operation.Consumes.Add("application/form-data");
            foreach (ApiParameterDescription parameterDescription in array)
            {
                ApiParameterDescription description = parameterDescription;
                IParameter parameter = operation.Parameters.First(o => o.Name == description.Name);
                int index = operation.Parameters.IndexOf(parameter);
                NonBodyParameter nonBodyParameter1 = new NonBodyParameter
                {
                    Name = parameter.Name,
                    In = "formData",
                    Description = parameter.Description,
                    Required = parameter.Required,
                    Type = "file"
                };
                NonBodyParameter nonBodyParameter2 = nonBodyParameter1;
                operation.Parameters[index] = nonBodyParameter2;
            }
        }
    }
}
