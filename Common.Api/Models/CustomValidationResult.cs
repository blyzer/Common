namespace Common.Api.Models
{
    /// <summary>
    /// Result of model valitation operation
    /// </summary>
    public class CustomValidationResult
    {
        /// <summary>
        /// Indicate if the validated if duplicated
        /// </summary>
        /// <returns></returns>
        public bool Duplicated { get; set; }
        public CustomValidationResult(bool duplicated)
        {
            this.Duplicated = duplicated;
        }
    }
}