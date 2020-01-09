using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MusicVault.Services.Helpers
{
    class EntityValidationException:Exception
    {
        public EntityValidationException():base()
        {

        }

        public EntityValidationException(string message):base(message)
        {

        }

        public EntityValidationException( string message, params object[] args) 
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
