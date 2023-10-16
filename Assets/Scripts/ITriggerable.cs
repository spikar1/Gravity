internal interface ITriggerable
{
    public string name { get; set; }

    void OnTrigger(Player player);
    void OnTriggerLeave(Player player);
    void OnTriggerArrive(Player player);
}