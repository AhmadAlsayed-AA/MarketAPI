using System;
namespace Market.Services.Helpers.Validation
{
	public class ValidationResponse
	{
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }
    }
    public class ValidationResponse<T>
    {
        public T ValidatedObject { get; set; }
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }

    }
}

