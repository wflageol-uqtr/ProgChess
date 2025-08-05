import { MoreHorizontal, Pencil, Trash } from "lucide-react";

import { Button } from "../ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "../ui/dropdown-menu";
import { DeleteDialog } from "../dialog/DeleteDialog";
import { useState } from "react";
import { Link } from "react-router";

interface StudentExerciseActionProps {
  id: number;
  deleteFn: () => void;
}

export default function StudentExerciseAction({
  id,
  deleteFn,
}: StudentExerciseActionProps) {
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
              <Link className="flex mr-2" to={`/admin/student/edit/${id}`}>
                <Pencil className="w-5 h-5 text-green-500 mr-2" />
                <span>Modifier l'étudiant</span>
              </Link>
            </DropdownMenuItem>
            <DropdownMenuSeparator />
            <DropdownMenuItem
              className="cursor-pointer"
              onClick={(e) => {
                e.stopPropagation();
                setOpenMenu(false);
                setOpenDialog(true);
              }}
            >
              <Trash className="w-5 h-5 text-red-500" />
              <span>Supprimer l'étudiant</span>
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
