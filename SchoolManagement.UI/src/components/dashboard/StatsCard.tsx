import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { cn } from "@/lib/utils";

interface StatsCardProps {
  title: string;
  value: string | number;
  description?: string;
  icon: React.ComponentType<{ className?: string }>;
  trend?: {
    value: number;
    isPositive: boolean;
  };
  className?: string;
}

export function StatsCard({
  title,
  value,
  description,
  icon: Icon,
  trend,
  className,
}: StatsCardProps) {
  return (
    <Card className={cn("card-gradient shadow-card hover:shadow-elevated glow-on-hover group relative overflow-hidden", className)}>
      {/* Animated background gradient */}
      <div className="absolute inset-0 bg-gradient-to-r from-brand-primary/5 via-transparent to-brand-accent/5 opacity-0 group-hover:opacity-100 transition-opacity duration-500" />
      
      <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2 relative z-10">
        <CardTitle className="text-sm font-medium text-muted-foreground group-hover:text-brand-primary transition-colors duration-300">
          {title}
        </CardTitle>
        <div className="p-2 rounded-lg bg-brand-primary/10 group-hover:bg-brand-primary/20 transition-colors duration-300">
          <Icon className="h-5 w-5 text-brand-primary group-hover:scale-110 transition-transform duration-300" />
        </div>
      </CardHeader>
      <CardContent className="relative z-10">
        <div className="text-3xl font-bold text-foreground group-hover:text-brand-primary transition-colors duration-300 mb-1">
          {value}
        </div>
        {description && (
          <p className="text-sm text-muted-foreground mb-2">{description}</p>
        )}
        {trend && (
          <div className="flex items-center">
            <div className={cn(
              "flex items-center px-2 py-1 rounded-full text-xs font-semibold",
              trend.isPositive 
                ? "bg-education-green/10 text-education-green" 
                : "bg-destructive/10 text-destructive"
            )}>
              <span>
                {trend.isPositive ? "↗" : "↘"} {Math.abs(trend.value)}%
              </span>
            </div>
            <span className="text-xs text-muted-foreground ml-2">vs last month</span>
          </div>
        )}
      </CardContent>
      
      {/* Subtle shimmer effect on hover */}
      <div className="absolute inset-0 -top-full bg-gradient-to-b from-transparent via-white/5 to-transparent group-hover:animate-shimmer" />
    </Card>
  );
}