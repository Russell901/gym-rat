using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gym_rat.Interfaces
{
    public interface INavigationService
    {
        Task NavigateToAsync(string route);
        Task NavigateBackAsync();
    }
}
