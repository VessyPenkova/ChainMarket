
using System.ComponentModel.DataAnnotations;
using ChainMarket.Web.Models;

namespace ChainMarket.Tests
{
    public class CheckoutViewModelTests
    {
        [Test]
        public void Checkout_WithValidCustomerDetails_ShouldPassValidation()
        {
            var model = CreateValidModel();

            Assert.That(Validate(model), Is.Empty);
        }

        [TestCase("CustomerName")]
        [TestCase("Email")]
        [TestCase("Phone")]
        [TestCase("DeliveryAddress")]
        public void Checkout_WithMissingRequiredField_ShouldFailValidation(string propertyName)
        {
            var model = CreateValidModel();

            typeof(CheckoutViewModel).GetProperty(propertyName)!.SetValue(model, "");

            var errors = Validate(model);

            Assert.That(errors.Any(e => e.MemberNames.Contains(propertyName)), Is.True);
        }

        [Test]
        public void Checkout_WithInvalidEmail_ShouldFailValidation()
        {
            var model = CreateValidModel();
            model.Email = "invalid-email";

            var errors = Validate(model);

            Assert.That(errors.Any(e => e.MemberNames.Contains(nameof(model.Email))), Is.True);
        }

        [Test]
        public void Checkout_WithInvalidPhone_ShouldFailValidation()
        {
            var model = CreateValidModel();
            model.Phone = "invalid-phone-number";

            var errors = Validate(model);

            Assert.That(errors.Any(e => e.MemberNames.Contains(nameof(model.Phone))), Is.True);
        }

        [Test]
        public void Checkout_WithNameExceedingMaximumLength_ShouldFailValidation()
        {
            var model = CreateValidModel();
            model.CustomerName = new string('A', 151);

            var errors = Validate(model);

            Assert.That(errors.Any(e => e.MemberNames.Contains(nameof(model.CustomerName))), Is.True);
        }

        [Test]
        public void Checkout_WithAddressExceedingMaximumLength_ShouldFailValidation()
        {
            var model = CreateValidModel();
            model.DeliveryAddress = new string('A', 301);

            var errors = Validate(model);

            Assert.That(errors.Any(e => e.MemberNames.Contains(nameof(model.DeliveryAddress))), Is.True);
        }


        [Test]
        public void Checkout_IndividualWithoutCompanyDetails_ShouldPassValidation()
        {
            var model = CreateValidModel();
            model.CustomerType = "Individual";

            Assert.That(Validate(model), Is.Empty);
        }

        [Test]
        public void Checkout_CompanyWithoutInvoiceDetails_ShouldFailValidation()
        {
            var model = CreateValidModel();
            model.CustomerType = "Company";

            var errors = Validate(model);

            Assert.Multiple(() =>
            {
                Assert.That(errors.Any(e => e.MemberNames.Contains(nameof(model.CompanyName))), Is.True);
                Assert.That(errors.Any(e => e.MemberNames.Contains(nameof(model.CompanyEik))), Is.True);
                Assert.That(errors.Any(e => e.MemberNames.Contains(nameof(model.BillingAddress))), Is.True);
            });
        }

        [Test]
        public void Checkout_CompanyWithValidInvoiceDetails_ShouldPassValidation()
        {
            var model = CreateValidModel();
            model.CustomerType = "Company";
            model.CompanyName = "Test Company Ltd.";
            model.CompanyEik = "123456789";
            model.BillingAddress = "Plovdiv, Bulgaria";

            Assert.That(Validate(model), Is.Empty);
        }

        private static CheckoutViewModel CreateValidModel()
        {
            return new CheckoutViewModel
            {
                CustomerName = "Test Customer",
                Email = "customer@example.com",
                Phone = "+359888123456",
                DeliveryAddress = "Plovdiv, Bulgaria"
            };
        }

        private static List<ValidationResult> Validate(CheckoutViewModel model)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, new ValidationContext(model), results, true);

            return results;
        }
    }
}
