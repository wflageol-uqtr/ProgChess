using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProChess.Server.Entities;

public class StudentExercise
{
    public int StudentId { get; set; }
    
    public int ExerciseId { get; set; }
    
    public Student Student { get; set; }
    
    public Exercise Exercise { get; set; }
    
    public bool IsComplete{ get; set; }
}