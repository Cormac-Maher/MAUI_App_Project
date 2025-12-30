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
        Option2Btn.IsVisible = true;      
        Option3Btn.IsVisible = true;
        Option4Btn.IsVisible = true;
    }
    private void LoadQuestion()
    {
        FeedbackLabel.Text = ""; 
                                                             
        _currentMovie = _movies[_random.Next(_movies.Count)]; 
        EmojiLabel.Text = _currentMovie.emoji; 
                                               
        var wrongAnswers = _movies .Where(m => m.title != _currentMovie.title) 
            .OrderBy(m => _random.Next()) .Take(3) .ToList();
                                                              
        var options = wrongAnswers.Append(_currentMovie) 
            .OrderBy(m => _random.Next()) .ToList();   
        Option1Btn.Text = options[0].title; 
        Option2Btn.Text = options[1].title; 
        Option3Btn.Text = options[2].title; 
        Option4Btn.Text = options[3].title;
    } 
    private async void Option_Clicked(object sender, EventArgs e) 
    { 
        var btn = (Button)sender; 
        string selected = btn.Text;

        var bgColor = (Color)Application.Current.Resources["AppBackgroundColor"];

        if (selected == _currentMovie.title) 
        { 
            FeedbackLabel.Text = "Correct!";
            if (bgColor == Colors.Green)
            {
                FeedbackLabel.TextColor = Colors.Black;
            }
            else
            {
                FeedbackLabel.TextColor = Colors.Green;
            }
        } 
        else
        { 
            FeedbackLabel.Text = $"Wrong! Correct answer: {_currentMovie.title}";
            if (bgColor == Colors.Red)
            {
                FeedbackLabel.TextColor = Colors.Black;
            }
            else
            {
                FeedbackLabel.TextColor = Colors.Red;
            } 
        } 
        await Task.Delay(1000);
        LoadQuestion();
    }
    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
   

