using System;
using System.Collections.Generic;

namespace SharpHaven.Game
{
	public class MessageBus : IMessagePublisher, IMessageSource
	{
		private readonly Dictionary<Type, object> handlers;

		public MessageBus()
		{
			this.handlers = new Dictionary<Type, object>();
		}

		public void Publish<TMessage>(TMessage message)
		{
			foreach (var handler in GetHandlersOf<TMessage>())
				handler(message);
		}

		public void Subscribe<TMessage>(MessageHandler<TMessage> handler)
		{
			GetHandlersOf<TMessage>().Add(handler);
		}

		public void Unsubscribe<TMessage>(MessageHandler<TMessage> handler)
		{
			GetHandlersOf<TMessage>().Remove(handler);
		}

		private ICollection<MessageHandler<T>> GetHandlersOf<T>()
		{
			object handlerList;
			if (!handlers.TryGetValue(typeof(T), out handlerList))
			{
				handlerList = new List<MessageHandler<T>>();
				handlers[typeof(T)] = handlerList;
			}
			return (ICollection<MessageHandler<T>>)handlerList;
		}
	}
}
