using System.Collections.Generic;
using Zenject;

namespace Core.Scripts.Installers
{
    public class ZoneWindowPool
    {
        private readonly ZoneWindow.Factory _factory;
        private readonly int _initialSize;
        private readonly List<ZoneWindow> _pool = new();

        [Inject]
        public ZoneWindowPool(ZoneWindow.Factory factory, int initialSize)
        {
            _factory = factory;
            _initialSize = initialSize;

            for (var i = 0; i < _initialSize; i++)
            {
                var newInstance = _factory.Create();
                newInstance.gameObject.SetActive(false);
                newInstance.Hide();

                _pool.Add(newInstance);
            }
        }

        public ZoneWindow GetZoneWindow()
        {
            foreach (var zoneWindow in _pool)
                if (!zoneWindow.gameObject.activeInHierarchy)
                {
                    zoneWindow.gameObject.SetActive(true);
                    return zoneWindow;
                }

            var newInstance = _factory.Create();
            _pool.Add(newInstance);
            return newInstance;
        }

        public void ReturnZoneWindow(ZoneWindow zoneWindow)
        {
            zoneWindow.gameObject.SetActive(false);
        }
    }
}