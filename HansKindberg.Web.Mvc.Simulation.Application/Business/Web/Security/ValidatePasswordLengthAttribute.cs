using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using HansKindberg.Web.Simulation.Applications.Logic.Web.Security;
using DependencyResolver = HansKindberg.Web.Simulation.Applications.Logic.IoC.DependencyResolver;

namespace HansKindberg.Web.Mvc.Simulation.Application.Business.Web.Security
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute, IClientValidatable
    {
        #region Fields

        private const string _defaultErrorMessageFormat = "'{0}' must be at least {1} characters long.";
        private readonly int _minimumCharacters;

        #endregion

        #region Constructors

        public ValidatePasswordLengthAttribute() : this(DependencyResolver.Instance.GetService<IMembershipProvider>().MinimumRequiredPasswordLength) {}

        public ValidatePasswordLengthAttribute(int minimumCharacters) : base(_defaultErrorMessageFormat)
        {
            this._minimumCharacters = minimumCharacters;
        }

        #endregion

        #region Properties

        public int MinimumCharacters
        {
            get { return this._minimumCharacters; }
        }

        #endregion

        #region Methods

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, name, this._minimumCharacters);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            if(metadata == null)
                throw new ArgumentNullException("metadata");

            return new[]
                {
                    new ModelClientValidationStringLengthRule(this.FormatErrorMessage(metadata.GetDisplayName()), this._minimumCharacters, int.MaxValue)
                };
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= this._minimumCharacters);
        }

        #endregion
    }
}