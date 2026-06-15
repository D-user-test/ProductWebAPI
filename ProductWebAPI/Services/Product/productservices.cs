namespace ProductWebAPI.Services.Product
{
    public class productservices:Iproductservices
    {
        private readonly IConfiguration _configuration;
        public productservices(IConfiguration configuration)
        {
            _configuration = configuration;
        }


    }
}
