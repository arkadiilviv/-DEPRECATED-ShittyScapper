namespace Services.Interfaces
{
    public interface IScrapper{
        public byte[] GetScrappedData();
        public object GetFormatedData();
        public Task<IScrapper> Scrap();
        public IScrapper AddParameter<T>(string param, T value);
        public IScrapper BuildParameters();
    }
}