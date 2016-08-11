using System;
using System.Collections.Generic;
using System.Reflection;
using Haven.Messaging.Messages;

namespace Haven.Messaging
{
	public class MessageHandlerBase : IDisposable
	{
		private static readonly Type[] SupportedMessageTypes = {
			typeof(UpdateAmbientLight),
			typeof(UpdateAstronomy),
			typeof(BuffClearAll),
			typeof(BuffRemove),
			typeof(BuffUpdate),
			typeof(UpdateCharAttributes),
			typeof(UpdateActions),
			typeof(UpdateGameTime),
			typeof(UpdateGameObject),
			typeof(MapInvalidate),
			typeof(MapInvalidateGrid),
			typeof(MapInvalidateRegion),
			typeof(MapUpdateGrid),
			typeof(PartyUpdateMember),
			typeof(PartyChangeLeader),
			typeof(PartyUpdate),
			typeof(PlayMusic),
			typeof(PlaySound),
			typeof(LoadResource),
			typeof(LoadTilesets),
			typeof(WidgetCreate),
			typeof(WidgetDestroy),
			typeof(WidgetMessage),
		};

		private readonly IMessageSource source;
		private readonly Dictionary<Type, object> handlers;

		public MessageHandlerBase(IMessageSource source)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			this.source = source;
			this.handlers = new Dictionary<Type, object>();

			InitializeHandlers();
			SubscribeAll();
		}

		public virtual void Dispose()
		{
			UnsubscribeAll();
		}

		private void InitializeHandlers()
		{
			foreach (var messageType in SupportedMessageTypes)
			{
				var handlerMethodInfo = GetType().GetMethod(
					nameof(Handle),
					BindingFlags.Instance | BindingFlags.NonPublic,
					Type.DefaultBinder,
					new[] { messageType },
					null);
				var handlerType = typeof(MessageHandler<>).MakeGenericType(messageType);
				handlers[messageType] = Delegate.CreateDelegate(handlerType, this, handlerMethodInfo);
			}
		}

		private void SubscribeAll()
		{
			var methodInfo = source.GetType().GetMethod(nameof(source.Subscribe));
			foreach (var entry in handlers)
			{
				var messageType = entry.Key;
				var handler = entry.Value;
				methodInfo.MakeGenericMethod(messageType).Invoke(source, new [] { handler });
			}
		}

		private void UnsubscribeAll()
		{
			var methodInfo = source.GetType().GetMethod(nameof(source.Unsubscribe));
			foreach (var entry in handlers)
			{
				var messageType = entry.Key;
				var handler = entry.Value;
				methodInfo.MakeGenericMethod(messageType).Invoke(source, new[] { handler });
			}
		}

		protected virtual void Handle(WidgetCreate message)
		{
		}

		protected virtual void Handle(WidgetMessage message)
		{
		}

		protected virtual void Handle(WidgetDestroy message)
		{
		}

		protected virtual void Handle(LoadResource message)
		{
		}

		protected virtual void Handle(LoadTilesets message)
		{
		}

		protected virtual void Handle(MapInvalidate message)
		{
		}

		protected virtual void Handle(MapInvalidateGrid message)
		{
		}

		protected virtual void Handle(MapInvalidateRegion message)
		{
		}

		protected virtual void Handle(UpdateCharAttributes message)
		{
		}

		protected virtual void Handle(UpdateGameTime message)
		{
		}

		protected virtual void Handle(UpdateAmbientLight message)
		{
		}

		protected virtual void Handle(UpdateAstronomy message)
		{
		}

		protected virtual void Handle(UpdateActions message)
		{
		}

		protected virtual void Handle(UpdateGameObject message)
		{
		}

		protected virtual void Handle(MapUpdateGrid message)
		{
		}

		protected virtual void Handle(BuffUpdate message)
		{
		}

		protected virtual void Handle(BuffRemove message)
		{
		}

		protected virtual void Handle(BuffClearAll message)
		{
		}

		protected virtual void Handle(PartyChangeLeader message)
		{
		}

		protected virtual void Handle(PartyUpdate message)
		{
		}

		protected virtual void Handle(PartyUpdateMember message)
		{
		}

		protected virtual void Handle(PlaySound message)
		{
		}

		protected virtual void Handle(PlayMusic message)
		{
		}
	}
}
