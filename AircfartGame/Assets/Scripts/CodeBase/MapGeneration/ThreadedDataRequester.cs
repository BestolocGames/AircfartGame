using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace CodeBase.MapGeneration
{
	public class ThreadedDataRequester : MonoBehaviour {

		static ThreadedDataRequester _instance;
		Queue<ThreadInfo> _dataQueue = new Queue<ThreadInfo>();

		void Awake() {
			_instance = FindObjectOfType<ThreadedDataRequester> ();
		}

		public static void RequestData(Func<object> generateData, Action<object> callback) {
			ThreadStart threadStart = delegate {
				_instance.DataThread (generateData, callback);
			};

			new Thread (threadStart).Start ();
		}

		void DataThread(Func<object> generateData, Action<object> callback) {
			object data = generateData ();
			lock (_dataQueue) {
				_dataQueue.Enqueue (new ThreadInfo (callback, data));
			}
		}
		

		void Update() {
			if (_dataQueue.Count > 0) {
				for (int i = 0; i < _dataQueue.Count; i++) {
					ThreadInfo threadInfo = _dataQueue.Dequeue ();
					threadInfo.Callback (threadInfo.Parameter);
				}
			}
		}

		struct ThreadInfo {
			public readonly Action<object> Callback;
			public readonly object Parameter;

			public ThreadInfo (Action<object> callback, object parameter)
			{
				this.Callback = callback;
				this.Parameter = parameter;
			}

		}
	}
}
