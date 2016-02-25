using System;
using Windows.UI.Xaml.Data;

namespace LinuxCommandsCollection.Converters
{
	internal class TitleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			string title;
			switch (value.ToString())
			{
				case "wjgl":
					title = "文件管理";
					break;

				case "cpgl":
					title = "磁盘管理";
					break;

				case "wdbj":
					title = "文档编辑";
					break;

				case "wjcs":
					title = "文件传输";
					break;

				case "cpwh":
					title = "磁盘维护";
					break;

				case "wltx":
					title = "网络通讯";
					break;

				case "xtgl":
					title = "系统管理";
					break;

				case "xtsz":
					title = "系统设置";
					break;

				case "bfys":
					title = "备份压缩";
					break;

				case "sbgl":
					title = "设备管理";
					break;

				default:
					title = "分类出错烦请反馈";
					break;
			}
			return title;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}