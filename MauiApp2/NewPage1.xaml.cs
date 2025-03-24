namespace MauiApp2;

public partial class NewPage1 : ContentPage
{
    public NewPage1()
    {
        InitializeComponent();
    }

    private void OnMondayInsertClicked(object sender, EventArgs e)
    {
        MondayBreakfastLabel.Text = MondayBreakfastEntry.Text;
        MondayLunchLabel.Text = MondayLunchEntry.Text;
        MondayDinnerLabel.Text = MondayDinnerEntry.Text;
    }

    private void OnTuesdayInsertClicked(object sender, EventArgs e)
    {
        TuesdayBreakfastLabel.Text = TuesdayBreakfastEntry.Text;
        TuesdayLunchLabel.Text = TuesdayLunchEntry.Text;
        TuesdayDinnerLabel.Text = TuesdayDinnerEntry.Text;
    }

    private void OnWednesdayInsertClicked(object sender, EventArgs e)
    {
        WednesdayBreakfastLabel.Text = WednesdayBreakfastEntry.Text;
        WednesdayLunchLabel.Text = WednesdayLunchEntry.Text;
        WednesdayDinnerLabel.Text = WednesdayDinnerEntry.Text;
    }

    private void OnThursdayInsertClicked(object sender, EventArgs e)
    {
        ThursdayBreakfastLabel.Text = ThursdayBreakfastEntry.Text;
        ThursdayLunchLabel.Text = ThursdayLunchEntry.Text;
        ThursdayDinnerLabel.Text = ThursdayDinnerEntry.Text;
    }

    private void OnFridayInsertClicked(object sender, EventArgs e)
    {
        FridayBreakfastLabel.Text = FridayBreakfastEntry.Text;
        FridayLunchLabel.Text = FridayLunchEntry.Text;
        FridayDinnerLabel.Text = FridayDinnerEntry.Text;
    }

    private void OnSaturdayInsertClicked(object sender, EventArgs e)
    {
        SaturdayBreakfastLabel.Text = SaturdayBreakfastEntry.Text;
        SaturdayLunchLabel.Text = SaturdayLunchEntry.Text;
        SaturdayDinnerLabel.Text = SaturdayDinnerEntry.Text;
    }

    private void OnSundayInsertClicked(object sender, EventArgs e)
    {
        SundayBreakfastLabel.Text = SundayBreakfastEntry.Text;
        SundayLunchLabel.Text = SundayLunchEntry.Text;
        SundayDinnerLabel.Text = SundayDinnerEntry.Text;
    }
}