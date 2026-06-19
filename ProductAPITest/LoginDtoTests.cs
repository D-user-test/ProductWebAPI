using ProductWebAPI.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProductAPITest
{
    public class LoginDtoTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Fact]
        public void Username_IsRequired()
        {
            var dto = new logindto { Password = "1234" };
            var results = ValidateModel(dto);

            Assert.Contains(results, r => r.MemberNames.Contains("Username"));
        }

        [Fact]
        public void Password_IsRequired()
        {
            var dto = new logindto { Username = "john" };
            var results = ValidateModel(dto);

            Assert.Contains(results, r => r.MemberNames.Contains("Password"));
        }

        [Fact]
        public void Username_MaxLengthExceeded_ReturnsValidationError()
        {
            var dto = new logindto { Username = new string('a', 51), Password = "1234" };
            var results = ValidateModel(dto);

            Assert.Contains(results, r => r.MemberNames.Contains("Username"));
        }
    }
}
