namespace SalesSystem.SharedKernel.Responses
{
    public class PagedResponse<TData> : Response<TData>
    {
        private const int DEFAULT_PAGE_SIZE = 10;
        private const int DEFAULT_PAGE = 1;

        protected PagedResponse()
        { }

        protected PagedResponse(
            TData? data,
            int totalCount,
            int currentPage = DEFAULT_PAGE,
            int pageSize = DEFAULT_PAGE_SIZE,
            int code = DEFAULT_SUCCESS_STATUS_CODE,
            string? message = null,
            List<string>? errors = null)
            : base(data, code, message, errors)
        {
            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }

        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public int PageSize { get; set; } = DEFAULT_PAGE_SIZE;
        public int TotalCount { get; set; }

        public static PagedResponse<TData> Success(
            TData? data,
            int totalCount,
            int currentPage = DEFAULT_PAGE,
            int pageSize = DEFAULT_PAGE_SIZE,
            int code = DEFAULT_SUCCESS_STATUS_CODE,
            string? message = DEFAULT_SUCCESS_MESSAGE)
        {
            return new PagedResponse<TData>(data, totalCount, currentPage, pageSize, code, message);
        }

        public new static PagedResponse<TData> Failure(
            List<string> errors,
            string? message = DEFAULT_ERROR_MESSAGE,
            int code = DEFAULT_ERROR_STATUS_CODE)
        {
            return new PagedResponse<TData>(default, 0, DEFAULT_PAGE, DEFAULT_PAGE_SIZE, code, message, errors);
        }
    }
}