using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading;

EventLog item = EventLog.GetEventLogs().Where(it => it.Log == "System").First();
item.EnableRaisingEvents = true;
item.EntryWritten += OnSystemEntryWritten;

while (true)
{
    Thread.Sleep(TimeSpan.FromSeconds(100));
}

void OnSystemEntryWritten(object sender, EntryWrittenEventArgs entry)
{
    Console.WriteLine(entry.Entry.Source);
    Console.WriteLine(entry.Entry.Message);

    long id = entry.Entry.InstanceId;
    EventLogQuery query = new("System", PathType.LogName, "*[System[EventID=" + id + "]]");
    EventRecord item = new EventLogReader(query).ReadEvent();
    int processId = item.ProcessId.Value;

    Console.WriteLine(processId);
}
