// SineRtdServer.cs
//
// (C) Datasim Education BV  2010

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using Excel=Microsoft.Office.Interop.Excel;

namespace Datasim
{
	/// <summary>
	/// Simple RTD server that returns the data following as sine function.
	/// The topic data is the scale factor and the sine function input increment.
	/// </summary>
	[Guid("1677CA5C-C700-43eb-A1F3-B9BD0992DDBA")]
	[ProgId("Datasim.SineRtdServer")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	public class SineRtdServer: Excel.IRtdServer
	{
		// The Excel callback object to notify Excel of updates.
		private Excel.IRTDUpdateEvent m_callback=null;

		// The dictionary that stores for each TopicID the SineRtdServerTopic object.
		private Dictionary<int, SineRtdServerTopic> m_topics=new Dictionary<int, SineRtdServerTopic>();

		// A timer that triggers updates.
		private System.Timers.Timer m_timer;

		/// <summary>
		/// Called when Excel starts the RTD server.
		/// </summary>
		/// <param name="callbackObject">The call back object.</param>
		/// <returns>Status code.</returns>
		int Excel.IRtdServer.ServerStart(Microsoft.Office.Interop.Excel.IRTDUpdateEvent callbackObject)
		{
			// Store the callback object.
			m_callback=callbackObject;

			// Initialise and start the timer.
			m_timer=new System.Timers.Timer(1000);
			m_timer.Elapsed+=TimerElapsed;
			m_timer.Start();
			return 1;
		}

		/// <summary>
		/// The timer was elapsed.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">The event arguments.</param>
		void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			// Stop the timer and notify Excel there is new data.
			if (m_callback!=null)
			{
				m_timer.Stop();
				m_callback.UpdateNotify();
			}
		}

		/// <summary>
		/// Called when Excel registers a parameter set (topic).
		/// </summary>
		/// <param name="topicID">The topic ID for this parameter set.</param>
		/// <param name="strings">The parameter set.</param>
		/// <param name="getNewValues">Indicates if excel should get new values.</param>
		/// <returns>The current value.</returns>
		object Excel.IRtdServer.ConnectData(int topicID, ref Array strings, ref bool getNewValues)
		{
			try
			{
				// Add a topic object if not already in the dictionary (should always be the case).
				if (!m_topics.ContainsKey(topicID)) m_topics.Add(topicID, new SineRtdServerTopic(ref strings));

				// Excel should retrieve new values.
				getNewValues=true;

				// Return the current data for this topic.
				return m_topics[topicID].GetData();
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		/// <summary>
		/// Called when Excel unregisters a parameter set (topic).
		/// </summary>
		/// <param name="topicID">The topic to unregister.</param>
		void Excel.IRtdServer.DisconnectData(int topicID)
		{
			// Remove the topic from the dictionary.
 			if (m_topics.ContainsKey(topicID))
			{
				m_topics[topicID].Dispose();
				m_topics.Remove(topicID);
			}
		}

		/// <summary>
		/// Called by Excel to see if the RTD server is still active.
		/// </summary>
		/// <returns>The status.</returns>
		int Excel.IRtdServer.Heartbeat()
		{
			// Always return OK.
			return 1;
		}

		/// <summary>
		/// Called by Excel to retrieve changed data.
		/// </summary>
		/// <param name="topicCount">The number of topics that are changed.</param>
		/// <returns>The array with new data.</returns>
		Array Excel.IRtdServer.RefreshData(ref int topicCount)
		{
			// Return for each topic, the topic ID and new data.
			object[,] data=new object[2, m_topics.Count];

			// Get data for each topic.
			int index=0;
			foreach (int topic in m_topics.Keys)
			{
				data[0, index]=topic;
				data[1, index]=m_topics[topic].GetData();
				index++;
			}

			// Set the topic count and restart the timer.
			topicCount=m_topics.Count;
			m_timer.Start();

			// Return the data.
			return data;
		}


		/// <summary>
		/// Called when the RTD server must terminate.
		/// </summary>
		void Excel.IRtdServer.ServerTerminate()
		{
			// Stop the timer.
			if (m_timer!=null)
			{
				m_timer.Stop();
				m_timer.Dispose();
				m_timer=null;
			}
		}

	}
}
