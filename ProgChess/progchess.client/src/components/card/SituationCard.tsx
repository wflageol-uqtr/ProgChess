import { type LucideIcon } from "lucide-react";

interface SituationCardPros {
  children: React.ReactNode;
  title: string;
  icon: LucideIcon;
}

export default function SituationCard({
  children,
  title,
  icon: Icon,
}: SituationCardPros) {
  return (
    <div className="bg-zinc-800 rounded-2xl h-full w-full flex flex-col overflow-hidden">
      <div className="flex justify-between w-full p-2 items-center bg-zinc-700 rounded-t-2xl">
        <div className="flex gap-2 items-center">
          <Icon className="text-green-500" />
          <h4 className="font-semibold text-xl">{title}</h4>
        </div>
      </div>

      <div className="overflow-auto">{children}</div>
    </div>
  );
}
