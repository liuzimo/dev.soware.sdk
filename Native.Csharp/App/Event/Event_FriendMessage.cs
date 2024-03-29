﻿using Native.Csharp.Sdk.Cqp.Interface;
using Native.Csharp.Sdk.Cqp.EventArgs;
using System.Collections;

namespace Native.Csharp.App.Event
{
	public class Event_FriendMessage : IEvent_FriendMessage
	{
		#region --公开方法--
		/// <summary>
		/// Type=201 好友已添加<para/>
		/// 处理好友已经添加事件
		/// </summary>
		/// <param name="sender">事件的触发对象</param>
		/// <param name="e">事件的附加参数</param>
		public void ReceiveFriendIncrease (object sender, CqFriendIncreaseEventArgs e)
		{
            // 本子程序会在酷Q【线程】中被调用，请注意使用对象等需要初始化(CoInitialize,CoUninitialize)。
            // 这里处理消息
            e.Handler = LuaEnv.LuaEnv.RunLua(
                "",
                "envent/ReceiveFriendIncrease.lua",
                new ArrayList() {
                    "fromqq", e.FromQQ,
                });


            //e.Handled = false;   // 关于返回说明, 请参见 "Event_FriendMessage.ReceiveFriendMessage" 方法
		}

		/// <summary>
		/// Type=301 收到好友添加请求<para/>
		/// 处理收到的好友添加请求
		/// </summary>
		/// <param name="sender">事件的触发对象</param>
		/// <param name="e">事件的附加参数</param>
		public void ReceiveFriendAddRequest (object sender, CqAddFriendRequestEventArgs e)
		{
            // 本子程序会在酷Q【线程】中被调用，请注意使用对象等需要初始化(CoInitialize,CoUninitialize)。
            // 这里处理消息
            e.Handler = LuaEnv.LuaEnv.RunLua(
                "",
                "envent/ReceiveFriednAddRequest.lua",
                new ArrayList() {
                    "fromqq", e.FromQQ,
                    "message",e.Message,
                    "tag",e.ResponseFlag,
                });
            
            //e.Handled = false;   // 关于返回说明, 请参见 "Event_ReceiveMessage.ReceiveFriendMessage" 方法
        }

		/// <summary>
		/// Type=21 好友消息<para/>
		/// 处理收到的好友消息
		/// </summary>
		/// <param name="sender">事件的触发对象</param>
		/// <param name="e">事件的附加参数</param>
		public void ReceiveFriendMessage (object sender, CqPrivateMessageEventArgs e)
		{
            // 本子程序会在酷Q【线程】中被调用，请注意使用对象等需要初始化(CoInitialize,CoUninitialize)。
            // 这里处理消息
            e.Handler = LuaEnv.LuaEnv.RunLua(
                "",
                "envent/ReceivePrivateMessage.lua",
                new ArrayList() {
                    "fromqq", e.FromQQ,
                    "message",e.Message,
                    "id",e.MsgId,
                });
			// e.Handled 相当于 原酷Q事件的返回值
			// 如果要回复消息，请调用api发送，并且置 true - 截断本条消息，不再继续处理 //注意：应用优先级设置为"最高"(10000)时，不得置 true
			// 如果不回复消息，交由之后的应用/过滤器处理，这里置 false  - 忽略本条消息
		}
		#endregion
	}
}
