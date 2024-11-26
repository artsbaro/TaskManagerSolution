using Swashbuckle.AspNetCore.Annotations;

namespace TaskManagerAPI.HttpObjects
{
    public class SimpleResponseObject<T>
    {
        [SwaggerSchema("Indicador de sucesso no processamento", ReadOnly = false)]
        public bool Success { get; set; }

        [SwaggerSchema("Dados retornados", ReadOnly = true)]
        public T Data { get; set; }

        [SwaggerSchema("Erros encontrados", ReadOnly = false)]
        public IEnumerable<string> Errors { get; set; }
    }

    public class SimpleResponseObject
    {
        [SwaggerSchema("Indicador de sucesso no processamento", ReadOnly = false)]
        public bool Success { get; set; }

        [SwaggerSchema("Dados retornados", ReadOnly = true)]
        public object Data { get; set; }

        [SwaggerSchema("Erros encontrados", ReadOnly = false)]
        public IEnumerable<string> Errors { get; set; }
    }
}
