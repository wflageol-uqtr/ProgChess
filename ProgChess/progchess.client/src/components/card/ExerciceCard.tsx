import { type LucideIcon } from "lucide-react";
import { Button } from "../ui/button";

interface BoxCardProps {
  children: React.ReactNode;
  title: string;
  icon: LucideIcon;
  canExecute: boolean;
}

export default function ExerciceCard({
  children,
  title,
  icon: Icon,
  canExecute,
}: BoxCardProps) {
  return (
    <div className="bg-zinc-800 rounded-2xl h-full w-full flex flex-col overflow-hidden">
      <div className="flex justify-between w-full p-2 items-center bg-zinc-700 rounded-t-2xl">
        <div className="flex gap-2 items-center">
          <Icon className="text-green-500" />
          <h4 className="font-semibold text-xl">{title}</h4>
        </div>
        {canExecute && (
          <Button className="bg-green-500 cursor-pointer hover:bg-green-700">
            Exécuter
          </Button>
        )}
      </div>

      {children}
    </div>
  );
}
