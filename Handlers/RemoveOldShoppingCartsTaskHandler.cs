using System.Linq;
using Orchard.Data;
using Orchard.Environment;
using Orchard.Logging;
using Orchard.Services;
using Orchard.Tasks.Scheduling;
using OShop.Models;

namespace OShop.Handlers {
    public class RemoveOldShoppingCartsTaskHandler : IScheduledTaskHandler, IOrchardShellEvents {
        internal const string RemoveOldShoppingCartsTaskType = "RemoveOldShoppingCarts";

        private readonly IScheduledTaskManager _taskManager;
        private readonly IRepository<ShoppingCartRecord> _shoppingCartRepository;
        private readonly IClock _clock;

        public RemoveOldShoppingCartsTaskHandler(
            IScheduledTaskManager taskManager,
            IRepository<ShoppingCartRecord> shoppingCartRepository,
            IClock clock) {
            _taskManager = taskManager;
            _shoppingCartRepository = shoppingCartRepository;
            _clock = clock;
        }

        public ILogger Logger { get; set; }

        public void Process(ScheduledTaskContext context) {
            if (context.Task.TaskType == RemoveOldShoppingCartsTaskType) {
                var oldCarts = _shoppingCartRepository
                    .Fetch(sc => sc.ModifiedUtc < _clock.UtcNow.AddMonths(-1), o => o.Asc(sc => sc.ModifiedUtc), 0, 100);

                foreach (var cart in oldCarts) {
                    _shoppingCartRepository.Delete(cart);
                }

                Logger.Information("Removed {0} old shopping carts at {1} utc",
                    oldCarts.Count(),
                    _clock.UtcNow);

                _taskManager.CreateTask(RemoveOldShoppingCartsTaskType, oldCarts.Count() == 100 ? _clock.UtcNow.AddMinutes(10) : _clock.UtcNow.AddDays(1), null);
            }
        }

        public void Activated() {
            if (!_taskManager.GetTasks(RemoveOldShoppingCartsTaskType).Any()) {
                _taskManager.CreateTask(RemoveOldShoppingCartsTaskType, _clock.UtcNow.AddMinutes(10), null);
            }
        }

        public void Terminating() {
        }
    }
}