import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Bell, Check, Trash2, BellOff, Filter } from "lucide-react";
import { toast } from "@/hooks/use-toast";

interface Notification {
  id: string;
  title: string;
  message: string;
  type: "info" | "warning" | "success" | "error";
  category: string;
  timestamp: string;
  read: boolean;
}

const mockNotifications: Notification[] = [
  {
    id: "1",
    title: "New Student Registration",
    message: "Alice Johnson has been registered for Class 10A",
    type: "success",
    category: "students",
    timestamp: "2 minutes ago",
    read: false,
  },
  {
    id: "2",
    title: "Low Attendance Alert",
    message: "David Wilson's attendance has dropped below 80%",
    type: "warning",
    category: "attendance",
    timestamp: "1 hour ago",
    read: false,
  },
  {
    id: "3",
    title: "Fee Payment Received",
    message: "Payment of $500 received from Emma Brown",
    type: "success",
    category: "fees",
    timestamp: "3 hours ago",
    read: true,
  },
  {
    id: "4",
    title: "Exam Schedule Updated",
    message: "Mid-term exam dates have been updated",
    type: "info",
    category: "exams",
    timestamp: "5 hours ago",
    read: true,
  },
  {
    id: "5",
    title: "Staff Leave Request",
    message: "John Smith has requested leave from 15th to 20th",
    type: "info",
    category: "staff",
    timestamp: "1 day ago",
    read: false,
  },
];

export default function Notifications() {
  const [notifications, setNotifications] = useState(mockNotifications);
  const [filter, setFilter] = useState<"all" | "unread">("all");

  const unreadCount = notifications.filter((n) => !n.read).length;

  const filteredNotifications = notifications.filter((n) => {
    if (filter === "unread") return !n.read;
    return true;
  });

  const handleMarkAsRead = (id: string) => {
    setNotifications(notifications.map((n) =>
      n.id === id ? { ...n, read: true } : n
    ));
    toast({
      title: "Marked as read",
      description: "Notification has been marked as read",
    });
  };

  const handleMarkAllAsRead = () => {
    setNotifications(notifications.map((n) => ({ ...n, read: true })));
    toast({
      title: "All marked as read",
      description: "All notifications have been marked as read",
    });
  };

  const handleDelete = (id: string) => {
    setNotifications(notifications.filter((n) => n.id !== id));
    toast({
      title: "Notification deleted",
      description: "Notification has been removed",
    });
  };

  const handleClearAll = () => {
    setNotifications([]);
    toast({
      title: "All notifications cleared",
      description: "All notifications have been removed",
    });
  };

  const getTypeColor = (type: string) => {
    switch (type) {
      case "success": return "bg-education-green";
      case "warning": return "bg-education-orange";
      case "error": return "bg-destructive";
      default: return "bg-primary";
    }
  };

  const getCategoryLabel = (category: string) => {
    return category.charAt(0).toUpperCase() + category.slice(1);
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-3">
          <Bell className="h-8 w-8 text-primary" />
          <div>
            <h1 className="text-3xl font-bold text-foreground">Notifications</h1>
            <p className="text-muted-foreground">
              {unreadCount > 0 ? `${unreadCount} unread notification${unreadCount > 1 ? 's' : ''}` : 'All caught up!'}
            </p>
          </div>
        </div>
        <div className="flex gap-2">
          {unreadCount > 0 && (
            <Button onClick={handleMarkAllAsRead} variant="outline">
              <Check className="mr-2 h-4 w-4" />
              Mark all as read
            </Button>
          )}
          {notifications.length > 0 && (
            <Button onClick={handleClearAll} variant="outline">
              <Trash2 className="mr-2 h-4 w-4" />
              Clear all
            </Button>
          )}
        </div>
      </div>

      <Tabs defaultValue="all" onValueChange={(v) => setFilter(v as "all" | "unread")}>
        <div className="flex items-center justify-between mb-4">
          <TabsList>
            <TabsTrigger value="all" className="gap-2">
              All
              <Badge variant="secondary">{notifications.length}</Badge>
            </TabsTrigger>
            <TabsTrigger value="unread" className="gap-2">
              Unread
              {unreadCount > 0 && (
                <Badge className="bg-primary">{unreadCount}</Badge>
              )}
            </TabsTrigger>
          </TabsList>
          <Button variant="ghost" size="sm">
            <Filter className="mr-2 h-4 w-4" />
            Filter by category
          </Button>
        </div>

        <TabsContent value="all" className="space-y-3">
          {filteredNotifications.length === 0 ? (
            <Card>
              <CardContent className="flex flex-col items-center justify-center py-12">
                <BellOff className="h-12 w-12 text-muted-foreground mb-4" />
                <p className="text-lg font-medium">No notifications</p>
                <p className="text-sm text-muted-foreground">You're all caught up!</p>
              </CardContent>
            </Card>
          ) : (
            filteredNotifications.map((notification) => (
              <Card
                key={notification.id}
                className={`transition-all ${
                  !notification.read ? "border-l-4 border-l-primary bg-muted/30" : ""
                }`}
              >
                <CardContent className="p-4">
                  <div className="flex items-start justify-between gap-4">
                    <div className="flex items-start gap-3 flex-1">
                      <div className={`w-2 h-2 rounded-full mt-2 ${getTypeColor(notification.type)}`} />
                      <div className="flex-1 space-y-1">
                        <div className="flex items-center gap-2">
                          <h3 className={`font-semibold ${!notification.read ? "text-foreground" : "text-muted-foreground"}`}>
                            {notification.title}
                          </h3>
                          <Badge variant="outline" className="text-xs">
                            {getCategoryLabel(notification.category)}
                          </Badge>
                        </div>
                        <p className="text-sm text-muted-foreground">{notification.message}</p>
                        <p className="text-xs text-muted-foreground">{notification.timestamp}</p>
                      </div>
                    </div>
                    <div className="flex items-center gap-2">
                      {!notification.read && (
                        <Button
                          variant="ghost"
                          size="sm"
                          onClick={() => handleMarkAsRead(notification.id)}
                        >
                          <Check className="h-4 w-4" />
                        </Button>
                      )}
                      <Button
                        variant="ghost"
                        size="sm"
                        onClick={() => handleDelete(notification.id)}
                      >
                        <Trash2 className="h-4 w-4" />
                      </Button>
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))
          )}
        </TabsContent>

        <TabsContent value="unread" className="space-y-3">
          {filteredNotifications.length === 0 ? (
            <Card>
              <CardContent className="flex flex-col items-center justify-center py-12">
                <Check className="h-12 w-12 text-education-green mb-4" />
                <p className="text-lg font-medium">All caught up!</p>
                <p className="text-sm text-muted-foreground">No unread notifications</p>
              </CardContent>
            </Card>
          ) : (
            filteredNotifications.map((notification) => (
              <Card
                key={notification.id}
                className="border-l-4 border-l-primary bg-muted/30"
              >
                <CardContent className="p-4">
                  <div className="flex items-start justify-between gap-4">
                    <div className="flex items-start gap-3 flex-1">
                      <div className={`w-2 h-2 rounded-full mt-2 ${getTypeColor(notification.type)}`} />
                      <div className="flex-1 space-y-1">
                        <div className="flex items-center gap-2">
                          <h3 className="font-semibold text-foreground">{notification.title}</h3>
                          <Badge variant="outline" className="text-xs">
                            {getCategoryLabel(notification.category)}
                          </Badge>
                        </div>
                        <p className="text-sm text-muted-foreground">{notification.message}</p>
                        <p className="text-xs text-muted-foreground">{notification.timestamp}</p>
                      </div>
                    </div>
                    <div className="flex items-center gap-2">
                      <Button
                        variant="ghost"
                        size="sm"
                        onClick={() => handleMarkAsRead(notification.id)}
                      >
                        <Check className="h-4 w-4" />
                      </Button>
                      <Button
                        variant="ghost"
                        size="sm"
                        onClick={() => handleDelete(notification.id)}
                      >
                        <Trash2 className="h-4 w-4" />
                      </Button>
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))
          )}
        </TabsContent>
      </Tabs>
    </div>
  );
}