using System;

namespace Tasks
{
    public class TaskLocker
    {
        private Action<Func<bool>> _addStep;
        private bool _locked;

        public bool Lock()
        {
            if (_locked) return false;
            _locked = true;
            return true;
        }

        public async ITask LockAsync()
        {
            if (_locked) await TaskUtils.While(() => _locked, _addStep);
            _locked = true;
        }

        public void Unlock()
        {
            _locked = false;
        }
    }
}