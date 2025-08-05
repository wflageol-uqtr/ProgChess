import type { ColumnDef } from "@tanstack/react-table";
import type { Score } from "../../utils/type";
import { useNavigate } from "react-router";
import api from "../../utils/api";
import { toast } from "sonner";
import ScoreAction from "./ScoreAction";
import { Button } from "../ui/button";
import { ArrowUpDown } from "lucide-react";
import { handleApiError } from "../../utils/apiErrorHandler";
import { Checkbox } from "../ui/checkbox";

export const ScoreColumns: ColumnDef<Score>[] = [
  {
    id: "select",
    header: ({ table }) => (
      <Checkbox
        checked={
          table.getIsAllPageRowsSelected() ||
          (table.getIsSomePageRowsSelected() && "indeterminate")
        }
        onCheckedChange={(value) => table.toggleAllPageRowsSelected(!!value)}
        aria-label="Select all"
      />
    ),
    cell: ({ row }) => (
      <Checkbox
        checked={row.getIsSelected()}
        onCheckedChange={(value) => row.toggleSelected(!!value)}
        aria-label="Select row"
      />
    ),
  },
  {
    accessorKey: "id",
    header: "Id",
  },
  {
    accessorFn: (row) => row.studentExercise.studentPermanentCode,
    id: "permanentCode",
    header: "Code permanent",
  },
  {
    accessorKey: "exercise",
    header: ({ column }) => {
      return (
        <Button
          variant="ghost"
          onClick={() => column.toggleSorting(column.getIsSorted() === "asc")}
        >
          Exercise
          <ArrowUpDown className="ml-2 h-4 w-4" />
        </Button>
      );
    },
    cell: ({ row }) => {
      const exercise = row.getValue<[]>("exercise");
      return <div>{exercise.id}</div>;
    },
  },
  {
    header: "Action",
    id: "actions",
    cell: ({ row }) => {
      const navigate = useNavigate();

      const handleDelete = async () => {
        try {
          const response = await api.delete(
            `/api/score/delete/${row.getValue("id")}`
          );
          toast.success(response.data, {
            className: "bg-green-100",
          });
          navigate(0);
        } catch (error) {
          handleApiError(error);
        }
      };
      return <ScoreAction id={row.getValue("id")} deleteFn={handleDelete} />;
    },
  },
];
