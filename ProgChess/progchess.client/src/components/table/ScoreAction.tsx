import { MoreHorizontal, Pencil, Trash } from "lucide-react";

import { Button } from "../../components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "../../components/ui/dropdown-menu";
import { DeleteDialog } from "../dialog/DeleteDialog";
import { useState } from "react";
import { Link } from "react-router";

interface ScoreActionProps {
  id: number;
  deleteFn: () => void;
}

export default function ScoreAction({ id, deleteFn }: ScoreActionProps) {
  const [openDialog, setOpenDialog] = useState(false);
  const [openMenu, setOpenMenu] = useState(false);

  return (
    <>
      <DropdownMenu open={openMenu} onOpenChange={setOpenMenu}>
        <DropdownMenuTrigger asChild>
          <Button variant="ghost" className="h-8 w-8 p-0">
            <span className="sr-only">Open menu</span>
            <MoreHorizontal className="h-4 w-4" />
          </Button>
        </DropdownMenuTrigger>
        {openMenu && (
          <DropdownMenuContent align="end" className="bg-zinc-900 text-white">
            <DropdownMenuItem>
              <Link className="flex mr-2" to={`/admin/exercise/edit/${id}`}>
                <Pencil className="w-5 h-5 text-green-500 mr-2" />
                <span>Modifier le score</span>
              </Link>
            </DropdownMenuItem>
            <DropdownMenuSeparator />
            <DropdownMenuItem
              onClick={(e) => {
                e.stopPropagation();
                setOpenMenu(false);
                setOpenDialog(true);
              }}
            >
              <Trash className="w-5 h-5 text-red-500" />
              <span>Supprimer le score</span>
            </DropdownMenuItem>
          </DropdownMenuContent>
        )}
      </DropdownMenu>
      <DeleteDialog
        open={openDialog}
        message=" Cette action est irréversible. Cela supprimera définitivement
            l'élément."
        onOpenChange={setOpenDialog}
        deleteFn={deleteFn}
      />
    </>
  );
}
