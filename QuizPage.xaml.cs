namespace MovieExplorer;

public partial class QuizPage : ContentPage
{
  
    private List<Movies> _movies; 
    private Movies _currentMovie; 
    private Random _random; 
    public QuizPage(List<Movies> movies) 
    { 
        InitializeComponent(); 
        _movies = movies; 
        _random = new Random(); 
    }
    private void StartQuizBtn_Clicked(object sender, EventArgs e) 
    { 
        StartQuizBtn.IsEnabled = false;
        StartQuizBtn.IsVisible = false;
        LoadQuestion(); 
        Option1Btn.IsVisible = true;
        Option2Btn.IsVisible = true;                         // buttons are invisible until quiz starts
        Option3Btn.IsVisible = true;
        Option4Btn.IsVisible = true;
    }
    private void LoadQuestion()
    {
        AnswerLabel.Text = ""; 
                                                             
        _currentMovie = _movies[_random.Next(_movies.Count)];             // picks 1 random movie to be correct answer
        EmojiLabel.Text = _currentMovie.emoji; 
                                               
        var wrongAnswers = _movies .Where(m => m.title != _currentMovie.title).OrderBy(m => _random.Next()).Take(3).ToList();     // picks 3 random wrong answers

        var options = wrongAnswers.Append(_currentMovie).OrderBy(m => _random.Next()).ToList();     // combines correct answer with wrong answers and shuffles them
        Option1Btn.Text = options[0].title; 
        Option2Btn.Text = options[1].title; 
        Option3Btn.Text = options[2].title; 
        Option4Btn.Text = options[3].title;
    } 
    private async void Option_Clicked(object sender, EventArgs e) 
    { 
        var btn = (Button)sender; 
        string selected = btn.Text;

        var bgColor = (Color)Application.Current.Resources["AppBackgroundColor"];              // Adjust answer colour if it clashes with background colour

        if (selected == _currentMovie.title) 
        {
            AnswerLabel.Text = "Correct!";
            if (bgColor == Colors.Green)
            {
                AnswerLabel.TextColor = Colors.Black;
            }
            else
            {
                AnswerLabel.TextColor = Colors.Green;
            }
        } 
        else
        {
            AnswerLabel.Text = $"Wrong! Correct answer: {_currentMovie.title}";
            if (bgColor == Colors.Red)
            {
                AnswerLabel.TextColor = Colors.Black;
            }
            else
            {
                AnswerLabel.TextColor = Colors.Red;
            } 
        } 
        await Task.Delay(1000);             // delays before going on the the next question
        LoadQuestion();
    }
    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
   

