using System;
using System.Data;

namespace ACSManager
{
	public class UserData
	{
		/// <summary>
		/// 사용자 아이디
		/// </summary>
		private string userID;
		public string UserID
		{
			get { return userID; }
            set { userID = value; }
		}

		/// <summary>
		/// 사용자 권한별 그룹
		/// </summary>
		private string userGroup;
		public string UserGroup
		{
			get { return userGroup; }
            set { userGroup = value; }
		}
	}
}