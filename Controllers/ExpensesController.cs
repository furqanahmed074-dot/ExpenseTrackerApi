using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    private readonly AppDbContext _context;
    public ExpensesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetExpenses()
    {
        var expenses = _context.Expenses.ToList();
        return Ok(expenses);
    }
    
    [HttpPost]
    public IActionResult PostExpenses(Expense newExpense)
    {
        _context.Expenses.Add(newExpense);
        _context.SaveChanges();
        return Ok(newExpense);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateExpense(int id , Expense updatedExpense)
    {
        var expense = _context.Expenses.Find(id);
        if(expense == null)
        {
            return NotFound();
        }
        expense.Amount = updatedExpense.Amount;
        expense.Category = updatedExpense.Category;
        _context.SaveChanges();
        return Ok(expense);
    }

    [HttpDelete("{id}")]
    public IActionResult RemoveExpense(int id)
    {
         var expense = _context.Expenses.Find(id);
         if(expense == null)
        {
            return NotFound();
        }
        _context.Expenses.Remove(expense);
        _context.SaveChanges();
        return Ok(200);
    }
}
