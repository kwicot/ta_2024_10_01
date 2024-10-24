using System.Collections.Generic;
using Zenject;

namespace Core.Scripts.Installers
{
    public class ZoneWindowPool
    {
        private List<ZoneWindow> _pool = new List<ZoneWindow>();
        private ZoneWindow.Factory _factory;
        private int _initialSize;

        [Inject]
        public ZoneWindowPool(ZoneWindow.Factory factory, int initialSize)
        {
            _factory = factory;
            _initialSize = initialSize;

            for (int i = 0; i < _initialSize; i++)
            {
                ZoneWindow newInstance = _factory.Create();
                newInstance.gameObject.SetActive(false);
                newInstance.Hide();
                
                _pool.Add(newInstance);
            }
        }

        public ZoneWindow GetZoneWindow()
        {
            foreach (var zoneWindow in _pool)
            {
                if (!zoneWindow.gameObject.activeInHierarchy)
                {
                    zoneWindow.gameObject.SetActive(true);
                    return zoneWindow;
                }
            }

            ZoneWindow newInstance = _factory.Create();
            _pool.Add(newInstance);
            return newInstance;
        }

        public void ReturnZoneWindow(ZoneWindow zoneWindow)
        {
            zoneWindow.gameObject.SetActive(false);
        }
    }
}