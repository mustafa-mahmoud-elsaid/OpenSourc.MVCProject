namespace Demo.PL.Controllers
{
    public class DepartmentsController : Controller
    {
        //private IGenericRepository<Department> _repo;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult>Index()
        {
            var departments = await _unitOfWork.Departments.GetAllAsync();
            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (!ModelState.IsValid) return View(department);
            await _unitOfWork.Departments.CreateAsync(department);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id) =>await DepartmentControllerHandler(id, nameof(Details));

        public async Task<IActionResult> Edit(int? id) => await DepartmentControllerHandler(id, nameof(Edit));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, Department department)
        {

            if (id != department.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Departments.Update(department);
                    await _unitOfWork.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(department);
        }

        public async Task<IActionResult> Delete(int? id) => await DepartmentControllerHandler(id, nameof(Delete));

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var department = await _unitOfWork.Departments.GetAsync(id.Value);
            if (department is null) return NotFound();

            try
            {
                _unitOfWork.Departments.Delete(department);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(department);
        }

        private async Task<IActionResult> DepartmentControllerHandler(int? id, string viewName)
        {
            if (!id.HasValue) return BadRequest();

            var department = await _unitOfWork.Departments.GetAsync(id.Value);
            if (department is null) return NotFound();

            return View(viewName, department);
        }
    }
}
