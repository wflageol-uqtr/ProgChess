import { RefreshCcw, type LucideIcon } from "lucide-react";
import { Button } from "../ui/button";
import { DeleteDialog } from "../dialog/DeleteDialog";
import { useState } from "react";

interface ExecutableCardProps {
  children: React.ReactNode;
  title: string;
  icon: LucideIcon;
  isPending?: boolean;
  actionFn?: () => void;
  reinitializeFn: () => void;
}

export default function ExecutableCard({
  children,
  title,
  icon: Icon,
  isPending,
  actionFn,
  reinitializeFn,
}: ExecutableCardProps) {
  const [openDeleteDialog, setOpenDeleteDialog] = useState(false);

  return (
    <>
      <div className="bg-zinc-800 rounded-2xl h-full w-full flex flex-col overflow-hidden">
        <div className="flex justify-between w-full p-2 items-center bg-zinc-700 rounded-t-2xl">
          <div className="flex gap-2 items-center">
            <Icon className="text-green-500" />
            <h4 className="font-semibold text-xl">{title}</h4>
          </div>
          <div className="flex items-center space-x-2">
            <Button
              className="bg-zinc-500 hover:bg-zinc-600 cursor-pointer"
              type="button"
              onClick={(e) => {
                e.stopPropagation();
                setOpenDeleteDialog(true);
              }}
            >
              <RefreshCcw />
              <div className="hidden md:flex">Réinitialiser</div>
            </Button>

            <Button
              disabled={isPending}
              className="bg-green-500 cursor-pointer hover:bg-green-700"
              onClick={actionFn}
            >
              {isPending ? <span>Exécution</span> : <span>Exécuter</span>}
            </Button>
          </div>
        </div>

        <div className="overflow-auto">{children}</div>
      </div>
      <DeleteDialog
        open={openDeleteDialog}
        message="Cette action est irréversible. Le code que vous avez jusqu'à présent sera perdu."
        onOpenChange={setOpenDeleteDialog}
        deleteFn={() => {
          reinitializeFn();
          setOpenDeleteDialog(false);
        }}
      />
    </>
  );
}
