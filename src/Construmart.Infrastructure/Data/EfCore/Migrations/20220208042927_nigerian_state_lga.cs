using Microsoft.EntityFrameworkCore.Migrations;

namespace Construmart.Infrastructure.Data.EfCore.Migrations
{
    public partial class nigerian_state_lga : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NigerianState",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalGovernmentAreas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<string>(type: "nvarchar(max)", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NigerianState", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "2d53660a-e427-41d9-aa81-7dc7d5fb4e72");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1e637971-1849-4d1e-bacd-9433a817d425", "AQAAAAEAACcQAAAAECs7ZVXwYCqnhazlT7oJ7ZL8X0DakNmxCDIrquvGEzV98CKahN6oBQrtSYzF4HaqXQ==", "16616986-dab1-4796-8d3d-13db89e2e3e1" });

            migrationBuilder.InsertData(
                table: "NigerianState",
                columns: new[] { "Id", "LocalGovernmentAreas", "State" },
                values: new object[,]
                {
                    { 36L, "[\"Bade\",\"Bursari\",\"Damaturu\",\"Fika\",\"Fune\",\"Geidam\",\"Gujba\",\"Gulani\",\"Jakusko\",\"Karasuwa\",\"Machina\",\"Nangere\",\"Nguru\",\"Potiskum\",\"Tarmuwa\",\"Yunusari\"]", "Yobe" },
                    { 22L, "[\"Aleiro\",\"Argungu\",\"Arewa Dandi\",\"Augie\",\"Bagudo\",\"Birnin Kebbi\",\"Bunza\",\"Dandi\",\"Fakai\",\"Gwandu\",\"Jega\",\"Kalgo\",\"Koko/Besse\",\"Maiyama\",\"Ngaski\",\"Shanga\",\"Suru\",\"Sakaba\",\"Wasagu/Danko\",\"Yauri\",\"Zuru\"]", "Kebbi" },
                    { 23L, "[\"Ajaokuta\",\"Adavi\",\"Ankpa\",\"Bassa\",\"Dekina\",\"Ibaji\",\"Idah\",\"Igalamela Odolu\",\"Ijumu\",\"Kogi\",\"Kabba/Bunu\",\"Lokoja\",\"Ofu\",\"Mopa Muro\",\"Ogori/Magongo\",\"Okehi\",\"Okene\",\"Olamaboro\",\"Omala\",\"Yagba East\",\"Yagba West\"]", "Kogi" },
                    { 24L, "[\"Asa\",\"Baruten\",\"Edu\",\"Ilorin East\",\"Ifelodun\",\"Ilorin South\",\"Ekiti Kwara State\",\"Ilorin West\",\"Irepodun\",\"Isin\",\"Kaiama\",\"Moro\",\"Offa\",\"Oke Ero\",\"Oyun\",\"Pategi\"]", "Kwara" },
                    { 25L, "[\"Agege\",\"Ajeromi-Ifelodun\",\"Alimosho\",\"Amuwo-Odofin\",\"Badagry\",\"Apapa\",\"Epe\",\"Eti Osa\",\"Ibeju-Lekki\",\"Ifako-Ijaiye\",\"Ikeja\",\"Ikorodu\",\"Kosofe\",\"Lagos Island\",\"Mushin\",\"Lagos Mainland\",\"Ojo\",\"Oshodi-Isolo\",\"Shomolu\",\"Surulere Lagos State\"]", "Lagos" },
                    { 26L, "[\"Akwanga\",\"Awe\",\"Doma\",\"Karu\",\"Keana\",\"Keffi\",\"Lafia\",\"Kokona\",\"Nasarawa Egon\",\"Nasarawa\",\"Obi\",\"Toto\",\"Wamba\"]", "Nasarawa" },
                    { 27L, "[\"Agaie\",\"Agwara\",\"Bida\",\"Borgu\",\"Bosso\",\"Chanchaga\",\"Edati\",\"Gbako\",\"Gurara\",\"Katcha\",\"Kontagora\",\"Lapai\",\"Lavun\",\"Mariga\",\"Magama\",\"Mokwa\",\"Mashegu\",\"Moya\",\"Paikoro\",\"Rafi\",\"Rijau\",\"Shiroro\",\"Suleja\",\"Tafa\",\"Wushishi\"]", "Niger" },
                    { 37L, "[\"Anka\",\"Birnin Magaji/Kiyaw\",\"Bakura\",\"Bukkuyum\",\"Bungudu\",\"Gummi\",\"Gusau\",\"Kaura Namoda\",\"Maradun\",\"Shinkafi\",\"Maru\",\"Talata Mafara\",\"Tsafe\",\"Zurmi\"]", "Zamfara" },
                    { 28L, "[\"Abeokuta North\",\"Abeokuta South\",\"Ado-Odo/Ota\",\"Egbado North\",\"Ewekoro\",\"Egbado South\",\"Ijebu North\",\"Ijebu East\",\"Ifo\",\"Ijebu Ode\",\"Ijebu North East\",\"Imeko Afon\",\"Ikenne\",\"Ipokia\",\"Odeda\",\"Obafemi Owode\",\"Odogbolu\",\"Remo North\",\"Ogun Waterside\",\"Shagamu\"]", "Ogun" },
                    { 30L, "[\"Aiyedire\",\"Atakunmosa West\",\"Atakunmosa East\",\"Aiyedaade\",\"Boluwaduro\",\"Boripe\",\"Ife East\",\"Ede South\",\"Ife North\",\"Ede North\",\"Ife South\",\"Ejigbo\",\"Ife Central\",\"Ifedayo\",\"Egbedore\",\"Ila\",\"Ifelodun\",\"Ilesa East\",\"Ilesa West\",\"Irepodun\",\"Irewole\",\"Isokan\",\"Iwo\",\"Obokun\",\"Odo Otin\",\"Ola Oluwa\",\"Olorunda\",\"Oriade\",\"Orolu\",\"Osogbo\"]", "Osun" },
                    { 31L, "[\"Afijio\",\"Akinyele\",\"Atiba\",\"Atisbo\",\"Egbeda\",\"Ibadan North\",\"Ibadan North-East\",\"Ibadan North-West\",\"Ibadan South-East\",\"Ibarapa Central\",\"Ibadan South-West\",\"Ibarapa East\",\"Ido\",\"Ibarapa North\",\"Irepo\",\"Iseyin\",\"Itesiwaju\",\"Iwajowa\",\"Kajola\",\"Lagelu\",\"Ogbomosho North\",\"Ogbomosho South\",\"Ogo Oluwa\",\"Olorunsogo\",\"Oluyole\",\"Ona Ara\",\"Orelope\",\"Ori Ire\",\"Oyo\",\"Oyo East\",\"Saki East\",\"Saki West\",\"Surulere Oyo State\"]", "Oyo" },
                    { 32L, "[\"Bokkos\",\"Barkin Ladi\",\"Bassa\",\"Jos East\",\"Jos North\",\"Jos South\",\"Kanam\",\"Kanke\",\"Langtang South\",\"Langtang North\",\"Mangu\",\"Mikang\",\"Pankshin\",\"Qua\\u0027an Pan\",\"Riyom\",\"Shendam\",\"Wase\"]", "Plateau" },
                    { 33L, "[\"Abua/Odual\",\"Ahoada East\",\"Ahoada West\",\"Andoni\",\"Akuku-Toru\",\"Asari-Toru\",\"Bonny\",\"Degema\",\"Emuoha\",\"Eleme\",\"Ikwerre\",\"Etche\",\"Gokana\",\"Khana\",\"Obio/Akpor\",\"Ogba/Egbema/Ndoni\",\"Ogu/Bolo\",\"Okrika\",\"Omuma\",\"Opobo/Nkoro\",\"Oyigbo\",\"Port Harcourt\",\"Tai\"]", "Rivers" },
                    { 34L, "[\"Gudu\",\"Gwadabawa\",\"Illela\",\"Isa\",\"Kebbe\",\"Kware\",\"Rabah\",\"Sabon Birni\",\"Shagari\",\"Silame\",\"Sokoto North\",\"Sokoto South\",\"Tambuwal\",\"Tangaza\",\"Tureta\",\"Wamako\",\"Wurno\",\"Yabo\",\"Binji\",\"Bodinga\",\"Dange Shuni\",\"Goronyo\",\"Gada\"]", "Sokoto" },
                    { 35L, "[\"Ardo Kola\",\"Bali\",\"Donga\",\"Gashaka\",\"Gassol\",\"Ibi\",\"Jalingo\",\"Karim Lamido\",\"Kumi\",\"Lau\",\"Sardauna\",\"Takum\",\"Ussa\",\"Wukari\",\"Yorro\",\"Zing\"]", "Taraba" },
                    { 21L, "[\"Bakori\",\"Batagarawa\",\"Batsari\",\"Baure\",\"Bindawa\",\"Charanchi\",\"Danja\",\"Dandume\",\"Dan Musa\",\"Daura\",\"Dutsi\",\"Dutsin Ma\",\"Faskari\",\"Funtua\",\"Ingawa\",\"Jibia\",\"Kafur\",\"Kaita\",\"Kankara\",\"Kankia\",\"Katsina\",\"Kurfi\",\"Kusada\",\"Mai\\u0027Adua\",\"Malumfashi\",\"Mani\",\"Mashi\",\"Matazu\",\"Musawa\",\"Rimi\",\"Sabuwa\",\"Safana\",\"Sandamu\",\"Zango\"]", "Katsina" },
                    { 29L, "[\"Akoko North-East\",\"Akoko North-West\",\"Akoko South-West\",\"Akoko South-East\",\"Akure North\",\"Akure South\",\"Ese Odo\",\"Idanre\",\"Ifedore\",\"Ilaje\",\"Irele\",\"Ile Oluji/Okeigbo\",\"Odigbo\",\"Okitipupa\",\"Ondo West\",\"Ose\",\"Ondo East\",\"Owo\"]", "Ondo" },
                    { 20L, "[\"Ajingi\",\"Albasu\",\"Bagwai\",\"Bebeji\",\"Bichi\",\"Bunkure\",\"Dala\",\"Dambatta\",\"Dawakin Kudu\",\"Dawakin Tofa\",\"Doguwa\",\"Fagge\",\"Gabasawa\",\"Garko\",\"Garun Mallam\",\"Gezawa\",\"Gaya\",\"Gwale\",\"Gwarzo\",\"Kabo\",\"Kano Municipal\",\"Karaye\",\"Kibiya\",\"Kiru\",\"Kumbotso\",\"Kunchi\",\"Kura\",\"Madobi\",\"Makoda\",\"Minjibir\",\"Nasarawa\",\"Rano\",\"Rimin Gado\",\"Rogo\",\"Shanono\",\"Takai\",\"Sumaila\",\"Tarauni\",\"Tofa\",\"Tsanyawa\",\"Tudun Wada\",\"Ungogo\",\"Warawa\",\"Wudil\"]", "Kano" },
                    { 18L, "[\"Auyo\",\"Babura\",\"Buji\",\"Biriniwa\",\"Birnin Kudu\",\"Dutse\",\"Gagarawa\",\"Garki\",\"Gumel\",\"Guri\",\"Gwaram\",\"Gwiwa\",\"Hadejia\",\"Jahun\",\"Kafin Hausa\",\"Kazaure\",\"Kiri Kasama\",\"Kiyawa\",\"Kaugama\",\"Maigatari\",\"Malam Madori\",\"Miga\",\"Sule Tankarkar\",\"Roni\",\"Ringim\",\"Yankwashi\",\"Taura\"]", "Jigawa" },
                    { 2L, "[\"Demsa\",\"Fufure\",\"Ganye\",\"Gayuk\",\"Gombi\",\"Grie\",\"Hong\",\"Jada\",\"Larmurde\",\"Madagali\",\"Maiha\",\"Mayo Belwa\",\"Michika\",\"Mubi North\",\"Mubi South\",\"Numan\",\"Shelleng\",\"Song\",\"Toungo\",\"Yola North\",\"Yola South\"]", "Adamawa" },
                    { 3L, "[\"Abak\",\"Eastern Obolo\",\"Eket\",\"Esit Eket\",\"Essien Udim\",\"Etim Ekpo\",\"Etinan\",\"Ibeno\",\"Ibesikpo Asutan\",\"Ibiono-Ibom\",\"Ikot Abasi\",\"Ika\",\"Ikono\",\"Ikot Ekpene\",\"Ini\",\"Mkpat-Enin\",\"Itu\",\"Mbo\",\"Nsit-Atai\",\"Nsit-Ibom\",\"Nsit-Ubium\",\"Obot Akara\",\"Okobo\",\"Onna\",\"Oron\",\"Udung-Uko\",\"Ukanafun\",\"Oruk Anam\",\"Uruan\",\"Urue-Offong/Oruko\",\"Uyo\"]", "Akwa Ibom" },
                    { 4L, "[\"Aguata\",\"Anambra East\",\"Anaocha\",\"Awka North\",\"Anambra West\",\"Awka South\",\"Ayamelum\",\"Dunukofia\",\"Ekwusigo\",\"Idemili North\",\"Idemili South\",\"Ihiala\",\"Njikoka\",\"Nnewi North\",\"Nnewi South\",\"Ogbaru\",\"Onitsha North\",\"Onitsha South\",\"Orumba North\",\"Orumba South\",\"Oyi\"]", "Anambra" },
                    { 5L, "[\"Alkaleri\",\"Bauchi\",\"Bogoro\",\"Damban\",\"Darazo\",\"Dass\",\"Gamawa\",\"Ganjuwa\",\"Giade\",\"Itas/Gadau\",\"Jama\\u0027are\",\"Katagum\",\"Kirfi\",\"Misau\",\"Ningi\",\"Shira\",\"Tafawa Balewa\",\"Toro\",\"Warji\",\"Zaki\"]", "Bauchi" },
                    { 6L, "[\"Agatu\",\"Apa\",\"Ado\",\"Buruku\",\"Gboko\",\"Guma\",\"Gwer East\",\"Gwer West\",\"Katsina-Ala\",\"Konshisha\",\"Kwande\",\"Logo\",\"Makurdi\",\"Obi\",\"Ogbadibo\",\"Ohimini\",\"Oju\",\"Okpokwu\",\"Oturkpo\",\"Tarka\",\"Ukum\",\"Ushongo\",\"Vandeikya\"]", "Benue" },
                    { 7L, "[\"Abadam\",\"Askira/Uba\",\"Bama\",\"Bayo\",\"Biu\",\"Chibok\",\"Damboa\",\"Dikwa\",\"Guzamala\",\"Gubio\",\"Hawul\",\"Gwoza\",\"Jere\",\"Kaga\",\"Kala/Balge\",\"Konduga\",\"Kukawa\",\"Kwaya Kusar\",\"Mafa\",\"Magumeri\",\"Maiduguri\",\"Mobbar\",\"Marte\",\"Monguno\",\"Ngala\",\"Nganzai\",\"Shani\"]", "Borno" },
                    { 8L, "[\"Brass\",\"Ekeremor\",\"Kolokuma/Opokuma\",\"Nembe\",\"Ogbia\",\"Sagbama\",\"Southern Ijaw\",\"Yenagoa\"]", "Bayelsa" },
                    { 19L, "[\"Birnin Gwari\",\"Chikun\",\"Giwa\",\"Ikara\",\"Igabi\",\"Jaba\",\"Jema\\u0027a\",\"Kachia\",\"Kaduna North\",\"Kaduna South\",\"Kagarko\",\"Kajuru\",\"Kaura\",\"Kauru\",\"Kubau\",\"Kudan\",\"Lere\",\"Makarfi\",\"Sabon Gari\",\"Sanga\",\"Soba\",\"Zangon Kataf\",\"Zaria\"]", "Kaduna" },
                    { 9L, "[\"Abi\",\"Akamkpa\",\"Akpabuyo\",\"Bakassi\",\"Bekwarra\",\"Biase\",\"Boki\",\"Calabar Municipal\",\"Calabar South\",\"Etung\",\"Ikom\",\"Obanliku\",\"Obubra\",\"Obudu\",\"Odukpani\",\"Ogoja\",\"Yakuur\",\"Yala\"]", "Cross River" },
                    { 11L, "[\"Abakaliki\",\"Afikpo North\",\"Ebonyi\",\"Afikpo South\",\"Ezza North\",\"Ikwo\",\"Ezza South\",\"Ivo\",\"Ishielu\",\"Izzi\",\"Ohaozara\",\"Ohaukwu\",\"Onicha\"]", "Ebonyi" },
                    { 12L, "[\"Akoko-Edo\",\"Egor\",\"Esan Central\",\"Esan North-East\",\"Esan South-East\",\"Esan West\",\"Etsako Central\",\"Etsako East\",\"Etsako West\",\"Igueben\",\"Ikpoba Okha\",\"Orhionmwon\",\"Oredo\",\"Ovia North-East\",\"Ovia South-West\",\"Owan East\",\"Owan West\",\"Uhunmwonde\"]", "Edo" },
                    { 13L, "[\"Ado Ekiti\",\"Efon\",\"Ekiti East\",\"Ekiti South-West\",\"Ekiti West\",\"Emure\",\"Gbonyin\",\"Ido Osi\",\"Ijero\",\"Ikere\",\"Ilejemeje\",\"Irepodun/Ifelodun\",\"Ikole\",\"Ise/Orun\",\"Moba\",\"Oye\"]", "Ekiti" },
                    { 14L, "[\"Awgu\",\"Aninri\",\"Enugu East\",\"Enugu North\",\"Ezeagu\",\"Enugu South\",\"Igbo Etiti\",\"Igbo Eze North\",\"Igbo Eze South\",\"Isi Uzo\",\"Nkanu East\",\"Nkanu West\",\"Nsukka\",\"Udenu\",\"Oji River\",\"Uzo Uwani\",\"Udi\"]", "Enugu" },
                    { 15L, "[\"Abaji\",\"Bwari\",\"Gwagwalada\",\"Kuje\",\"Kwali\",\"Municipal Area Council\"]", "FCT Abuja" },
                    { 16L, "[\"Akko\",\"Balanga\",\"Billiri\",\"Dukku\",\"Funakaye\",\"Gombe\",\"Kaltungo\",\"Kwami\",\"Nafada\",\"Shongom\",\"Yamaltu/Deba\"]", "Gombe" },
                    { 17L, "[\"Aboh Mbaise\",\"Ahiazu Mbaise\",\"Ehime Mbano\",\"Ezinihitte\",\"Ideato North\",\"Ideato South\",\"Ihitte/Uboma\",\"Ikeduru\",\"Isiala Mbano\",\"Mbaitoli\",\"Isu\",\"Ngor Okpala\",\"Njaba\",\"Nkwerre\",\"Nwangele\",\"Obowo\",\"Oguta\",\"Ohaji/Egbema\",\"Okigwe\",\"Orlu\",\"Orsu\",\"Oru East\",\"Oru West\",\"Owerri Municipal\",\"Owerri North\",\"Unuimo\",\"Owerri West\"]", "Imo" },
                    { 10L, "[\"Aniocha North\",\"Aniocha South\",\"Bomadi\",\"Burutu\",\"Ethiope West\",\"Ethiope East\",\"Ika North East\",\"Ika South\",\"Isoko North\",\"Isoko South\",\"Ndokwa East\",\"Ndokwa West\",\"Okpe\",\"Oshimili North\",\"Oshimili South\",\"Patani\",\"Sapele\",\"Udu\",\"Ughelli North\",\"Ukwuani\",\"Ughelli South\",\"Uvwie\",\"Warri North\",\"Warri South\",\"Warri South West\"]", "Delta" },
                    { 1L, "[\"Aba North\",\"Arochukwu\",\"Aba South\",\"Bende\",\"Isiala Ngwa North\",\"Ikwuano\",\"Isiala Ngwa South\",\"Isuikwuato\",\"Obi Ngwa\",\"Ohafia\",\"Osisioma\",\"Ugwunagbo\",\"Ukwa East\",\"Ukwa West\",\"Umuahia North\",\"Umuahia South\",\"Umu Nneochi\"]", "Abia" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NigerianState");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "ConcurrencyStamp",
                value: "0de4869c-25e2-4f92-adf4-d1a14ad8bbba");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "da5ab3a6-9292-450b-9e23-a06ac9022c3d", "AQAAAAEAACcQAAAAECHrMyVlJUxDHLV4J1W6mxBpad5KoBwaa5QJSRDWA0u5Vqlvlwb5AHN+lunVCXnGJw==", "20560e8e-e72f-4c5f-b1ca-cda77a44b01e" });
        }
    }
}
