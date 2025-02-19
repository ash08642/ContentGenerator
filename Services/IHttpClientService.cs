namespace ContentGenerator.Services;

public interface IHttpClientService
{
    public Task<string> GemeniGetReq(string text, int inputLength);
}