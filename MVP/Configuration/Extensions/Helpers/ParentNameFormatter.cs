using MassTransit;

namespace POC.Configuration.Extensions.Helpers
{
    class ParentNameFormatter :
        IEntityNameFormatter
    {

        public string FormatEntityName<T>()
        {
            return GetDomain(typeof(T));
        }

        public static string GetDomain(Type obj)
        {
            Console.WriteLine($"[ParentNameFormatter]: {obj}");
            return obj.FullName.Replace("POC.Events.", "").Split(".").First();
        }
    }

}
