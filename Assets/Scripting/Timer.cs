using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DN
{
    public class Timer
    {
        public bool IsRunning
        {
            get { return _running; }
        }

        public float Elapsed
        {
            get { return _elapsed; }
        }

        public EventHandler OnTick;

        public float Duration;

        private bool _running;
        private float _elapsed;

        public void Run(bool reset = false)
        {
            _running = true;
            if (reset)
            {
                _elapsed = 0f;
            }
        }

        public void Update(float dt)
        {
            if (!_running)
            {
                return;
            }

            _elapsed += dt;
            if (_elapsed >= Duration)
            {
                _elapsed = 0;
                _running = false;
                if (OnTick != null)
                {
                    OnTick(this, EventArgs.Empty);
                }
            }
        }
    }
}