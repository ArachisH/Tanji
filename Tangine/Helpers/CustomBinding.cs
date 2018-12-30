using System.Threading;
using System.Globalization;
using System.Windows.Forms;

namespace Tangine.Helpers
{
    public class CustomBinding : Binding
    {
        private readonly object _converterParameter;
        private readonly IValueConverter _converter;
        private readonly CultureInfo _converterCulture;

        public CustomBinding(string propertyName, object dataSource, string dataMember, IValueConverter valueConverter, object converterParameter = null)
            : this(propertyName, dataSource, dataMember, valueConverter, Thread.CurrentThread.CurrentUICulture, converterParameter)
        { }
        public CustomBinding(string propertyName, object dataSource, string dataMember, IValueConverter valueConverter, CultureInfo culture, object converterParameter = null)
            : base(propertyName, dataSource, dataMember, true)
        {
            _converter = valueConverter;
            _converterCulture = culture;
            _converterParameter = converterParameter;
        }

        protected override void OnParse(ConvertEventArgs cevent)
        {
            if (_converter != null)
            {
                cevent.Value = _converter.ConvertBack(cevent.Value,
                    cevent.DesiredType, _converterParameter, _converterCulture);
            }
            else base.OnParse(cevent);
        }
        protected override void OnFormat(ConvertEventArgs cevent)
        {
            if (_converter != null)
            {
                cevent.Value = _converter.Convert(cevent.Value,
                    cevent.DesiredType, _converterParameter, _converterCulture);
            }
            else base.OnFormat(cevent);
        }
    }
}