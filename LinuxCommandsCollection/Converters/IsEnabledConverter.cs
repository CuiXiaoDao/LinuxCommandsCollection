using LinuxCommandsCollection.Data;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;

namespace LinuxCommandsCollection.Converters
{
	internal class IsEnabledConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			//if (parameter.ToString()=="删除")
			//{
			//	return ((List<Command>)value).Count != 0;
			//}
			return (int)value != 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}