import type { ColumnDef } from "@tanstack/react-table";
import type { Score } from "../../utils/type";
import { useNavigate } from "react-router";
import api from "../../utils/api";
import { toast } from "sonner";
import ScoreAction from "./ScoreAction";
import { Button } from "../ui/button";
import { ArrowUpDown } from "lucide-react";

export const ScoreColumns: ColumnDef<Score>[] = [
  {
    accessorKey: "id",
    header: "Id",
  },
  {
    accessorKey: "permanentCode",
    header: "Code permanent",
  },
  {
    accessorKey: "scoreValue",
    header: "Score",
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
          toast.error("Une erreur est survenue.");
        }
      };
      return <ScoreAction id={row.getValue("id")} deleteFn={handleDelete} />;
    },
  },
];
