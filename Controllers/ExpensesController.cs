using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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
    List<ExpenseReadDto> expenses_list = [];
        for (int i = 0 ; i < expenses.Count; i++)
        {
            ExpenseReadDto dto = new ExpenseReadDto();
            dto.Amount = expenses[i].Amount;
            dto.Category = expenses[i].Category;
            dto.Id = expenses[i].Id; 
            expenses_list.Add(dto);    
        }
        return Ok(expenses_list);
    }
    
    [HttpPost]
    public IActionResult PostExpenses(ExpenseCreateDto createDto)
    {
        Expense expense = new Expense();
        expense.Amount = createDto.Amount;
        expense.Category = createDto.Category;
        _context.Expenses.Add(expense);
        _context.SaveChanges();
        return Ok(createDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateExpense(int id ,  ExpenseCreateDto updateDto)
    {
        var expense = _context.Expenses.Find(id);
        if(expense == null)
        {
            return NotFound();
        }
        expense.Amount = updateDto.Amount;
        expense.Category = updateDto.Category;
        _context.SaveChanges();
        return Ok(updateDto);
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
