// File: NordicKeyLightWave.cs
// Program: RFIDInventory
// Author: Pavel Nikitin
// Licence: GPL 1.0
// Version 1.2

using System;
using NordicId;

namespace ru.nikitin.RFIDInventory
{
    // Imaging scan progress
    public class NordicKeyLightWave : IDisposable
    {
        // wave constants
        const int FRONT_COUNT = 2;
        const int MAX_LIGHT = 100;
        const int LIGHT_STEP = 10;
        const int TIMER_STEP  = 50;

        // wave state
        String[][] m_waveKeys = new string[FRONT_COUNT][];
        int m_front = 0;
        int[] m_frontLight = { 0, 0 };
        bool m_reverse = false;

        // timer
        System.Windows.Forms.Timer m_timer;

        // MHL KeyBacklight driver
        MHLDriver m_keyboard;


        // Constructor
        public NordicKeyLightWave()
        {
            // init wave front keys
            m_waveKeys[0] = new string[4] { "left", "up", "right", "down" };
            m_waveKeys[1] = new string[7] { "circle", "tab", "ok", "scan", "del", "esc", "square" };

            // init MHL KeyBacklight driver
            m_keyboard = new MHLDriver();
            m_keyboard.Open("KeyBacklight");

            if (m_keyboard.IsOpen())
                Reset();

            // key lite wave imaging timer
            m_timer = new System.Windows.Forms.Timer();
            m_timer.Interval = TIMER_STEP;
            m_timer.Tick += new System.EventHandler(this.NextStep);
        }


        // Imaging switcher
        public bool Enabled
        {
            get
            {
                return m_timer.Enabled;
            }

            set
            {
                if (m_keyboard.IsOpen())
                {
                    if (!value)
                        Reset();

                    m_timer.Enabled = value;
                }
            }
        }

        // Reverse wave
        public bool Reverse
        {
            get
            {
                return m_reverse;
            }

            set
            {
                if (m_reverse != value)
                {
                    string[] swap = m_waveKeys[0];
                    m_waveKeys[0] = m_waveKeys[1];
                    m_waveKeys[1] = swap;
                }

                m_reverse = value;
            }
        }

       
        // Reset wave
        public void Reset()
        {
            m_front = 0;
            for (int front = 0; front < m_waveKeys.Length; front++)
                SetWaveFrontLight(front, 0);
        }


        // IDisposable method
        public void Dispose()
        {
            Reset();
            m_keyboard.Close();
            m_timer.Dispose();
        }


        // Keys wave effect timer event
        public void NextStep(object sender, EventArgs e)
        {
            if (!m_keyboard.IsOpen())
                return;

            if (m_front < m_frontLight.Length)
            {
                if (m_frontLight[m_front] < MAX_LIGHT)
                {
                    SetWaveFrontLight(m_front, m_frontLight[m_front] + LIGHT_STEP);

                    if (m_front > 0 && m_frontLight[m_front - 1] > 0)
                        SetWaveFrontLight(m_front - 1, m_frontLight[m_front - 1] - LIGHT_STEP);
                }
                else
                    m_front++;
            }
            else
            {
                if (m_frontLight[m_front - 1] > 0)
                    SetWaveFrontLight(m_front - 1, m_frontLight[m_front - 1] - LIGHT_STEP);
                else
                    m_front = 0;
            }
        }

        // KeyBacklight 
        private void SetWaveFrontLight(int front, int light)
        {
            m_frontLight[front] = light;
            for (int i = 0; i < m_waveKeys[front].Length; i++)
                m_keyboard.SetDword("KeyBacklight." + m_waveKeys[front][i], light);
        }
    }
}
